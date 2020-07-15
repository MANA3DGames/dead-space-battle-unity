using UnityEngine;
using System.Collections;
using MANA3D.Utilities.Coroutine;

public class Enemy : Spaceship
{
    public class EnemyInfo
    {
        public Rank rank;
        public ApproachStyle appStyle;
        public DodgeStyle dodgStyle;
        public AttackStyle attackStyle;
        public FleeStyle fleeStyle;

        public bool canSwitchBehavior;
    }


    // ********************** Enum ************************* \\

    #region **** Enumerator ****

    public enum Rank
    {
        Mine,
        Soldier,
        Officer,
        General,
        Boss
    }

    public enum ApproachStyle
    {
        Enclosure,
        SameQuarter,
        DifferentQuarterUpperLower,
        DifferentQuarterLeftRight,
        DifferentQuarterAll
    }

    public enum DodgeStyle
    {
        StepSide,
        ChangeQuarter,
        Disappear
    }

    public enum AttackStyle
    {
        DirectContact,
        Normal_Shooter,
        Blade_Hurricane,
        Laser_Extended,
        RocketExtended
    }

    public enum FleeStyle
    {
        BackFlee,
        FrontFlee,
        SuicidalAtPosition,
        SuicidalWhereTarget,
        BackFleeShooter
    }

    #endregion


    // ********************* Global Variables *********************** \\

    #region ....... Public Variables .......

    public AudioClip gunSFX;
    public Transform gunTransform;
    public Light gunLight;
    public GameObject gunBullet;
    public GameObject gunSparks;
    public Transform gunSpawnPoint;

    public GameObject suicidalBombPrefab;
    public GameObject guiPoint;

    #endregion

    #region ....... Private Variables .......

    private Vector3 _targetPos;
    private Vector3 _targetLookAt;

    private EnemyInfo _info;

    private CoroutineTask _lifeCycleTask;

    private System.Func<VoidDelegate> _mainFunc;

    #endregion

    #region ....... Private References .......


    #endregion


    // *********************** AI-Functionality ************************ \\

    #region /|||||||||||| MonoBehaviour ||||||||||||\

    protected override void OnEnable()
    {
        base.OnEnable();
        if ( _lifeCycleTask == null )
            _lifeCycleTask = new CoroutineTask( lifeCycleCoroutine(), true );
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if ( _lifeCycleTask != null )
        {
            _lifeCycleTask.kill();
            _lifeCycleTask = null;
        }
    }

    protected override void onGamePausedHandler( bool pause )
    {
        base.onGamePausedHandler( pause );

        if ( _lifeCycleTask != null )
        {
            if ( pause )
            {
                //if ( _lifeCycleTask.IsRunning )
                    _lifeCycleTask.pause();
            }
            else
            {
                //if ( _lifeCycleTask.IsPaused )
                    _lifeCycleTask.unPause();
            }
        }
    }

    /// <summary>
    /// Initialize Player.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        _info = new EnemyInfo();

        _mainFunc = approach;

        if ( _lifeCycleTask == null )
            _lifeCycleTask = new CoroutineTask( lifeCycleCoroutine(), true );
    }


    public void CreateWeapon( int initAmmo, int max, float damage, float rate, float speed  )
    {
        CreateWeapon( initAmmo, max, damage, rate, speed, 
                      gunSFX, gunTransform, gunLight, gunBullet, gunSpawnPoint, name + "_Bullets", tag );
    }

    public void CreateWeapon( int initAmmo, int max, float damage, float rate, float speed,
                             AudioClip sfx, Transform gunTrans, Light lit, GameObject bullet, Transform spawn,
                             string packageName, string ignoreTag, float sfxVolume = 0.1f )
    {
        //defaultGun = new BasicGun( 100000, 100000, 10, 0.5f, 200, gunSFX, gunTransform, gunLight, gunBullet, gunSpawnPoint, name + "_Bullets", tag );
        defaultGun = new BasicGun( initAmmo, max, damage, rate, speed, gunSFX, gunTransform, gunLight, gunBullet, gunSpawnPoint, name + "_Bullets", tag );
        defaultGun.setAsEndless( true );
    }


    /// <summary>
    /// Update spaceship position, rotation according to AI input.
    /// </summary>
    protected virtual void Update()
    {
        // Do not proceed if it is not active.
        if ( !_isActivated || _isPaused ) return;

        // Call main function according the current state.
        _mainFunc.Invoke();
    }

    protected override void OnTriggerEnter( Collider other )
    {
        base.OnTriggerEnter( other );

        if ( other.tag == "Player" )
        {
            destroy();
        }
    }

    #endregion



    #region /|||||||||||| Approach-Style Functions ||||||||||||\


    /// <summary>
    /// Checks whether to make spaceship approach a target position or not.
    /// </summary>
    /// <returns>returns true if we need this spaceship to approach a target</returns>
    bool checkApproach()
    {
        return false;
    }
    VoidDelegate approach()
    {
        switch ( _info.appStyle )
        {
            case ApproachStyle.Enclosure:
                updateTransformation( updateEnclosurePosition( 1.0f ), 3.0f, 0.0f, 1.0f );
                break;
            case ApproachStyle.SameQuarter:
            case ApproachStyle.DifferentQuarterUpperLower:
            case ApproachStyle.DifferentQuarterLeftRight:
            case ApproachStyle.DifferentQuarterAll:
                updateTargetPosition( 30 );
                updateTransformation( _targetPos, 3.0f, 0.0f, 1.0f );
                break;
        }

        return null;
    }


    /// <summary>
    /// Updates the target position after each certain amount of frames.
    /// </summary>
    /// <param name="updateRate">Number of frames</param>
    void updateTargetPosition( int updateRate )
    {
        // Check if number of frames has passed.
        if ( Time.frameCount % updateRate == 0 )
            // Find a random position in the same quarter as the target (Player).
            _targetPos = findRandomTargetPosition();
    }

    void updateTransformation( Vector3 targetLookAt, float lookSpeed, float hVal, float vVal )
    {
        // Update movement and rotation.
        lookAt( targetLookAt, lookSpeed );
        move( hVal, vVal );
    }


    /// <summary>
    /// Rotates spaceship around a dynamic position.
    /// </summary>
    /// <param name="direction">rotation direction ( Clockwise = 1 ) ( CounterClockwise = -1 )</param>
    /// <returns>returns next target position as a vector3</returns>
    Vector3 updateEnclosurePosition( float direction )
    {
        // Calcuate target position according to a moving object.
        Vector3 targetPos = Player.Instance.transform.position;
        float xOffset = 50 * Mathf.Cos(direction * 1.0f * Time.time);
        float zOffset = 50 * Mathf.Sin(direction * 1.0f * Time.time);
        targetPos = new Vector3(xOffset + targetPos.x, 1.5f, zOffset + targetPos.z);

        return targetPos;
    }

    /// <summary>
    /// Finds a random position according to the quarter that the player is inside now.
    /// </summary>
    /// <returns>Returns the target position as a vector3</returns>
    Vector3 findRandomTargetPosition()
    {
        // Create an empty vector3 to save the target position.
        Vector3 targetLookAt = Vector3.zero;

        int xPos = 0, zPos = 0;

        // Check which quarter the player is inside now.
        decideTargetPosition( ref xPos, ref zPos );

        // Set the componenet values for the vector3 which is going to be the target position for this enemy.
        targetLookAt = new Vector3( xPos, 0.0f, zPos );

        // Return the final target position.
        return targetLookAt;
    }

    /// <summary>
    /// Finds a random position according to the current approch style.
    /// </summary>
    /// <param name="xPos">Reference for the x value of the vector3 that will store the target position</param>
    /// <param name="zPos">Reference for the Y value of the vector3 that will store the target position</param>
    void decideTargetPosition( ref int xPos, ref int zPos )
    {
        // Get the current quarter that the player is inside now.
        LevelManager.QuarterArea quarter = Player.Instance.CurrentQuarter;

        switch ( _info.appStyle )
        {
            case ApproachStyle.Enclosure:
                break;
            case ApproachStyle.SameQuarter:
                // Check which quarter the player is inside now.
                switch (quarter)
                {
                    case LevelManager.QuarterArea.UpperRight:
                        xPos = Random.Range( 0, (int)GameManager.Instance.GetLevelLimitation(1) );
                        zPos = Random.Range( 0, (int)GameManager.Instance.GetLevelLimitation(3) );
                        break;
                    case LevelManager.QuarterArea.UpperLeft:
                        xPos = Random.Range( (int)GameManager.Instance.GetLevelLimitation(0), 0 );
                        zPos = Random.Range( 0, (int)GameManager.Instance.GetLevelLimitation(3) );
                        break;
                    case LevelManager.QuarterArea.LowerLeft:
                        xPos = Random.Range( (int)GameManager.Instance.GetLevelLimitation(0), 0 );
                        zPos = Random.Range( (int)GameManager.Instance.GetLevelLimitation(2), 0 );
                        break;
                    case LevelManager.QuarterArea.LowerRight:
                        xPos = Random.Range( 0, (int)GameManager.Instance.GetLevelLimitation(1) );
                        zPos = Random.Range( (int)GameManager.Instance.GetLevelLimitation(2), 0 );
                        break;
                    case LevelManager.QuarterArea.Unknown:
                        xPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(0), (int)GameManager.Instance.GetLevelLimitation(1));
                        zPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(2), (int)GameManager.Instance.GetLevelLimitation(3));
                        break;
                }
                break;
            case ApproachStyle.DifferentQuarterUpperLower:
                // Check which quarter the player is inside now.
                switch (quarter)
                {
                    case LevelManager.QuarterArea.UpperRight:
                        xPos = Random.Range(0, (int)GameManager.Instance.GetLevelLimitation(1));
                        zPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(2), 0);
                        break;
                    case LevelManager.QuarterArea.UpperLeft:
                        xPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(0), 0);
                        zPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(2), 0);
                        break;
                    case LevelManager.QuarterArea.LowerLeft:
                        xPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(0), 0);
                        zPos = Random.Range(0, (int)GameManager.Instance.GetLevelLimitation(3));

                        break;
                    case LevelManager.QuarterArea.LowerRight:
                        xPos = Random.Range(0, (int)GameManager.Instance.GetLevelLimitation(1));
                        zPos = Random.Range(0, (int)GameManager.Instance.GetLevelLimitation(3));
                        break;
                    case LevelManager.QuarterArea.Unknown:
                        xPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(0), (int)GameManager.Instance.GetLevelLimitation(1));
                        zPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(2), (int)GameManager.Instance.GetLevelLimitation(3));
                        break;
                }
                break;
            case ApproachStyle.DifferentQuarterLeftRight:
                // Check which quarter the player is inside now.
                switch (quarter)
                {
                    case LevelManager.QuarterArea.UpperRight:
                        xPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(0), 0);
                        zPos = Random.Range(0, (int)GameManager.Instance.GetLevelLimitation(3));
                        break;
                    case LevelManager.QuarterArea.UpperLeft:
                        xPos = Random.Range(0, (int)GameManager.Instance.GetLevelLimitation(1));
                        zPos = Random.Range(0, (int)GameManager.Instance.GetLevelLimitation(3));
                        break;
                    case LevelManager.QuarterArea.LowerLeft:
                        xPos = Random.Range(0, (int)GameManager.Instance.GetLevelLimitation(1));
                        zPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(2), 0);
                        break;
                    case LevelManager.QuarterArea.LowerRight:
                        xPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(0), 0);
                        zPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(2), 0);
                        break;
                    case LevelManager.QuarterArea.Unknown:
                        xPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(0), (int)GameManager.Instance.GetLevelLimitation(1));
                        zPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(2), (int)GameManager.Instance.GetLevelLimitation(3));
                        break;
                }
                break;
            case ApproachStyle.DifferentQuarterAll:
                // Check which quarter the player is inside now.
                switch (quarter)
                {
                    case LevelManager.QuarterArea.UpperRight:
                        xPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(0), 0);
                        zPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(2), 0);
                        break;
                    case LevelManager.QuarterArea.UpperLeft:
                        xPos = Random.Range(0, (int)GameManager.Instance.GetLevelLimitation(1));
                        zPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(2), 0);
                        break;
                    case LevelManager.QuarterArea.LowerLeft:
                        xPos = Random.Range(0, (int)GameManager.Instance.GetLevelLimitation(1));
                        zPos = Random.Range(0, (int)GameManager.Instance.GetLevelLimitation(3));
                        break;
                    case LevelManager.QuarterArea.LowerRight:
                        xPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(0), 0);
                        zPos = Random.Range(0, (int)GameManager.Instance.GetLevelLimitation(3));
                        break;
                    case LevelManager.QuarterArea.Unknown:
                        xPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(0), (int)GameManager.Instance.GetLevelLimitation(1));
                        zPos = Random.Range((int)GameManager.Instance.GetLevelLimitation(2), (int)GameManager.Instance.GetLevelLimitation(3));
                        break;
                }
                break;
        }
    }

    #endregion


    #region /|||||||||||| Dodge-Style Functions ||||||||||||\

    /// <summary>
    /// Checks whether this spaceship needs to avoid something or not. 
    /// </summary>
    /// <returns>Returns true if this spaceship needs to avoid something. </returns>
    bool checkDodge()
    {
        return false;
    }
    VoidDelegate dodge()
    {
        return null;
    }

    #endregion


    #region /|||||||||||| Attack-Style Functions ||||||||||||\

    /// <summary>
    /// Checks if this spaceship can attack a target. 
    /// </summary>
    /// <returns>returns true if it can attack</returns>
    bool checkAttack()
    {
        return false;
    }
    VoidDelegate attack()
    {
        switch ( _info.attackStyle )
        {
            case AttackStyle.DirectContact:
                updateDirectContactAttack();
                break;
            case AttackStyle.Normal_Shooter:
                updateNormalShooter();
                break;
            case AttackStyle.Blade_Hurricane:
                updateBladeHurricane();
                break;
        }

        return null;
    }



    enum DirectContactState { MoveForward, MoveBack, RotateAround  }
    DirectContactState _directContactState;

    void updateDirectContactAttack()
    {
        switch ( _directContactState )
        {
            case DirectContactState.MoveForward:
                updateTransformation(Player.Instance.transform.position, 4.0f, 0.0f, 1.0f);
                break;
            case DirectContactState.MoveBack:
                updateTransformation(Player.Instance.transform.position, 4.0f, 0.0f, -1.0f);
                break;
            case DirectContactState.RotateAround:
                updateTransformation(updateEnclosurePosition(1.0f), 3.0f, 0.0f, 1.0f);
                break;
        }
        
    }

    IEnumerator directContactProcedure( int attackNum )
    {
        _info.appStyle = ApproachStyle.Enclosure;

        while ( attackNum > 0 )
        {
            _directContactState = DirectContactState.MoveForward;

            while ( Vector3.Distance(myTransform.position, Player.Instance.transform.position) > 20.0f )
            {
                yield return new WaitForSeconds( 0.1f );
            }

            _directContactState = DirectContactState.MoveBack;

            yield return new WaitForSeconds( 0.5f );

            _directContactState = DirectContactState.RotateAround;

            yield return new WaitForSeconds( 0.5f );


            attackNum--;
        }


        // Flee or Change behavior!!
        //allOff();
        //_isFleeing = true;
        _targetPos = findRandomFleePoint_Front();
    }


    void updateNormalShooter()
    {
        // Ship transformation.
        updateTargetPosition(30);
        updateTransformation(_targetPos, 3.0f, 0.0f, 1.0f);
        // Fire rotation.
        lookAtY_Inner(Player.Instance.transform.position, 30.0f);
        fire();
    }
    
    protected void fire()
    {
        // Check if we can fire.
        if ( Player.Instance.gameObject.activeSelf && defaultGun.canFire() )
        {
            // Call fire for the default gun.
            defaultGun.fire();
        }
    }


    enum BladeHurricaneState { InitBlade, ChargeBlade, MoveToTarget }
    BladeHurricaneState _bladeHurricaneState;

    float _bladeRotateSpeed = 0.0f;
    float _bladeRotateAccel = 5.0f;
    float _bladeRotateLimit = 100;//5000;

    void updateBladeHurricane()
    {
        if ( Mathf.Abs( _bladeRotateSpeed ) < Mathf.Abs( _bladeRotateLimit ) )
            _bladeRotateSpeed += _bladeRotateAccel;

        switch ( _bladeHurricaneState )
        {
            case BladeHurricaneState.InitBlade:
                // Show blazes!!.
                break;
            case BladeHurricaneState.ChargeBlade:

                break;
            case BladeHurricaneState.MoveToTarget:
                updateTransformation( _targetPos, 3.0f, 0.0f, 1.0f );
                break;
        }
        

        rotateInner( 0.0f, _bladeRotateSpeed, 0.0f );
        fire();
    }

    IEnumerator bladeHurricaneProcedure( int attackNum )
    {
        _info.appStyle = ApproachStyle.SameQuarter;

        
        _bladeHurricaneState = BladeHurricaneState.InitBlade;
        yield return new WaitForSeconds(1.0f);


        while ( attackNum > 0 )
        {
            _bladeHurricaneState = BladeHurricaneState.ChargeBlade;
            _bladeRotateAccel = 15.0f;
            _bladeRotateLimit = 100;// 5000;
            while ( _bladeRotateSpeed < 80 )//3000 )
            {
                defaultGun.setFireRate( defaultGun.GetFireRate() + 0.01f );
                yield return new WaitForSeconds(0.1f);
            }

            _bladeHurricaneState = BladeHurricaneState.MoveToTarget;
            _targetPos = Player.Instance.transform.position;
            setMoveSpeed(200);
            int counter = 0;
            while ( Vector3.Distance(myTransform.position, _targetPos) > 20.0f && counter < 80 )
            {
                counter++;
                yield return new WaitForSeconds(0.1f);
                Debug.Log(counter);
            }
                
            setMoveSpeed(40);
            yield return new WaitForSeconds(1.0f);


            _bladeHurricaneState = BladeHurricaneState.ChargeBlade;
            _bladeRotateAccel = -15.0f;
            while (_bladeRotateSpeed > 10)
                yield return new WaitForSeconds(0.1f);


            attackNum--;
        }


        // Flee or Change behavior!!
        //allOff();
        //_isFleeing = true;
        _targetPos = findRandomFleePoint_Front();
    }



    #endregion


    #region /|||||||||||| Flee-Style Functions ||||||||||||\

    /// <summary>
    /// Checks whether this ship needs to flee or not
    /// </summary>
    /// <returns>returns true if it should flee</returns>
    bool checkFlee()
    {
        return false;
    }
    VoidDelegate flee()
    {
        switch (_info.fleeStyle)
        {
            case FleeStyle.BackFlee:
            case FleeStyle.FrontFlee:
                updateTransformation(_targetPos, 3.0f, 0.0f, 1.0f);
                break;
            case FleeStyle.SuicidalAtPosition:
                approach();
                break;
            case FleeStyle.SuicidalWhereTarget:
                updateTransformation(_targetPos, 3.0f, 0.0f, 1.0f);
                break;
            case FleeStyle.BackFleeShooter:
                lookAtY_Inner(Player.Instance.transform.position, 4.0f);
                updateTransformation(_targetPos, 3.0f, 0.0f, 1.0f);
                break;
        }

        return null;
    }


    Vector3 findRandomFleePoint_Back()
    {
        return new Vector3( Random.Range((int)GameManager.Instance.GetLevelLimitation(0), (int)GameManager.Instance.GetLevelLimitation(1)), 
                            0.0f,
                            GameManager.Instance.GetLevelLimitation(2) * 4 );
    }

    Vector3 findRandomFleePoint_Front()
    {
        return new Vector3( Random.Range((int)GameManager.Instance.GetLevelLimitation(0), (int)GameManager.Instance.GetLevelLimitation(1)),
                            0.0f,
                            GameManager.Instance.GetLevelLimitation(3) * 4 );
    }


    IEnumerator suicideBomb( int tryNum )
    {
        int count = 0;
        Color color = Color.white;

        while ( count < tryNum )
        {
            if ( color == Color.white )
            {
                setInnerColor(Color.red);
                color = Color.red; 
            }
            else
            {
                setInnerColor(Color.white);
                color = Color.white; 
            }

            if ( _info.fleeStyle == FleeStyle.SuicidalWhereTarget )
            {
                if ( Vector3.Distance(myTransform.position, _targetPos) < 10.0f )
                    count = tryNum;
            }
            count++;
            yield return new WaitForSeconds( 0.1f );
        }


        //Instantiate(suicidalBombPrefab, myTransform.position, myTransform.rotation);
        //reset();
        destroy();
    }

    #endregion


    IEnumerator lifeCycleCoroutine()
    {
        _info.appStyle = (ApproachStyle)Random.Range( 0, 5 );
        yield return new WaitForSeconds( Random.Range( 3, 4 ) );

        _mainFunc = attack;
        _info.attackStyle = (AttackStyle)Random.Range( 0, 3 );
        if ( _info.attackStyle == AttackStyle.Blade_Hurricane )
            defaultGun.setFireRate( 0.2f );
        yield return new WaitForSeconds( Random.Range( 10, 15 ) );

        _mainFunc = flee;
        _info.fleeStyle = (FleeStyle)Random.Range( 0, 5 );
        yield return new WaitForSeconds( 10 );
        sendToRecycle();
    }

    public override void reset()
    {
        base.reset();

        //allOff();
        //myTransform.position = new Vector3(0.0f, 0.0f, 70.0f);
        //myTransform.rotation = Quaternion.identity;
        //resetInnerRotation();

        setInnerColor( Color.white );

        _mainFunc = approach;
        _info.appStyle = ApproachStyle.Enclosure;
        _info.attackStyle = AttackStyle.DirectContact;
        _info.dodgStyle = DodgeStyle.StepSide;
        _info.fleeStyle = FleeStyle.FrontFlee;

        defaultGun.setFireRate( 0.5f );

        _bladeRotateSpeed = 0.0f;
        _bladeRotateAccel = 5.0f;
        _bladeRotateLimit = 100;// 5000;
    }

    protected override void applyDamage( float damage )
    {
        base.applyDamage( damage );
        destroy();
    }

    protected override void destroy()
    {
        base.destroy();

        Instantiate( suicidalBombPrefab, myTransform.position, myTransform.rotation );
        GameManager.Instance.SpawnPlus10Point( myTransform.position - ( Vector3.right * 15 ) - ( Vector3.forward * 20 ) );
        GameManager.Instance.UpdatePlayerScore( 10 );
        //Destroy( gameObject );
        sendToRecycle();
    }

    public void sendToRecycle()
    {
        reset();
        GameManager.Instance.OnRecycleEnemy( myTransform.position );
        SendMessage( "recycle" );
    }

}