using UnityEngine;
using System.Collections;
using MANA3D.Utilities.Coroutine;

public class Player : Spaceship
{
    // ****************************************************************************
    // Start with default weapon, in addition to 1 shot of a super powerful weapon.
    // ****************************************************************************



    public static Player Instance;          // Public static player instance.


    //private BasicGun _defaultGun;
    private BasicGun _superGun;

    public AudioClip gunSFX;
    public AudioClip superGunSFX;
    public Transform gunTransform;
    public Light gunLight;
    public GameObject gunBullet;
    public GameObject superGunBullet;
    public GameObject gunSparks;
    public Transform gunSpawnPoint;

    public GameObject ragdoll;

    //Transform _lookAtTarget;                 // Empty object transform, which will be used to rotation.

    Animator _myAnim;


    bool _isInvincible = false;
    bool _isDead = false;

    //float _invincibleDuration;
    CoroutineTask _invincibleTask;
    CoroutineTask _superFireRateTask;
    CoroutineTask _reviveTask;


    public LevelManager.QuarterArea CurrentQuarter { get { return _currentQuarter; } }
    LevelManager.QuarterArea _currentQuarter;


    float _verticalVal, _horizontalVal = 0.0f;


    GameObject[] _itemGOs;



    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        // assign this instance to the static Instance.
        Instance = this;
    }


    /// <summary>
    /// Initialize Player.
    /// </summary>
    protected override void Start()
    {
        // Call Start function in the base class.
        base.Start();


        // Cache Animator component.
        _myAnim = myTransform.GetChild(0).FindChild("SciFi_LOW_fbx").GetComponent<Animator>();


        // Create a defualt gun.
        defaultGun = new BasicGun( 100000, 100000, 10, 0.2f, 300, gunSFX, gunTransform, gunLight, gunBullet, gunSpawnPoint, name + "_Bullets", tag, 0.3f );
        defaultGun.setAsEndless( true );

        _superGun = new BasicGun( 0, 1000000, 20, 0.15f, 400, superGunSFX, gunTransform, gunLight, superGunBullet, gunSpawnPoint, name + "_SuperBullets", tag, 0.4f );

        // Set move values.
        setDefualtMoveProperties();
        setMoveAccel( 10.0f );
        setMoveSpeed( 110.0f );

        _itemGOs = new GameObject[1];
        for ( int i = 0; i < _itemGOs.Length; i++ )
        {
            _itemGOs[i] = myTransform.FindChild( i.ToString() ).gameObject;
            _itemGOs[i].SetActive( false );
        }

        // Create an empty game object, this object will be used for rotation.
        //GameObject go = new GameObject( "lookAtTarget" );
        //_lookAtTarget = go.transform;
    }


    /// <summary>
    /// Update spaceship position, rotation according to the player input that comes
    /// from the InputManager class.
    /// </summary>
    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
            ApplyPowerUp( 0 );
        if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
            ApplyPowerUp( 1 );
        if ( Input.GetKeyDown( KeyCode.Alpha3 ) )
            ApplyPowerUp( 2 );
        if ( Input.GetKeyDown( KeyCode.Alpha4 ) )
            GameManager.Instance.UpdatePlayerLifes( 1 );


        // Do not proceed if it is not active.
        if ( !_isActivated || _isPaused || _isDead ) return;

        // Get current area of the player.
        _currentQuarter = getCurrentQuarter();

        // Get player movement input form the InputManager.
        float rawVertical = GameManager.Instance.GetInputVerticalVal();
        float rawHorizontal = GameManager.Instance.GetInputHorizontalVal();

        _verticalVal = Mathf.Abs( rawVertical ) > 0.0f ? rawVertical : Mathf.Lerp( _verticalVal, 0.0f, 5.0f * Time.deltaTime );
        _horizontalVal = Mathf.Abs( rawHorizontal ) > 0.0f ? rawHorizontal : Mathf.Lerp( _horizontalVal, 0.0f, 5.0f * Time.deltaTime );

        // Move the spaceship according to the current inputs.
        move( _horizontalVal, _verticalVal );

        // Make sure the player's spaceship doesn't go out of the level boundaries.
        clampPlayerPosition();

        // Rotate the player's spaceship around the z-axis.
        //rotateInnerBody();

        // Get fire input.
        if ( GameManager.Instance.GetInputIsFiring() ||
             GameManager.Instance.GetInputIsAiming() )
        {
            if ( GameManager.Instance.GetInputIsFiring() )
                processFiring();

            // Get rotation input.
            Vector2 look = GameManager.Instance.GetInputLookAt();

            // Make inner body lookAt the new lookAtTarget position.
            lookAtY_Inner_local( new Vector3( look.x, 0, look.y ), 10.0f );
        }
        else
        {
            _myAnim.SetBool( "isFiring", false );
        }

        // Offset the position according to the rotation input (look vector).
        applyFloatEffect();
    }

    void processFiring()
    {
        // Check if we can fire.
        if ( _superGun.canFire() )
        {
            _myAnim.SetBool( "isFiring", true );
            //_myAnim.CrossFade("HipFire[Rifle]", 0.1f);

            // Call fire for the default gun.
            _superGun.fire();

            return;
        }
        else if ( _superGun.GetAmmoCount() > 0 )
            return;

        if ( defaultGun.canFire() )
        {
            _myAnim.SetBool( "isFiring", true );
            //_myAnim.CrossFade("HipFire[Rifle]", 0.1f);

            // Call fire for the default gun.
            defaultGun.fire();
        }  
        else
        {
            //_myAnim.SetBool("isFiring", false);
            //_myAnim.CrossFade("Idle[Rifle]", 0.1f);
            //  Debug.Log("connot FIRE!!");
        }
    }


    /// <summary>
    /// Make sure the player's spaceship doesn't go out of the level limitation.
    /// </summary>
    void clampPlayerPosition()
    {
        // Cache the current position of the player.
        Vector3 clampPos = myTransform.position;

        // Check if the player's spaceship has exceeded the horizontal limitation. 
        if ( clampPos.x >= GameManager.Instance.GetLevelLimitation(1) || 
             clampPos.x <= GameManager.Instance.GetLevelLimitation(0) )
        {
            // Reset the horizotal velocity.
            resetHorizontalVelocity();
        }

        // Check if the player's spaceship has exceeded the vertical limitation. 
        if ( clampPos.z >= GameManager.Instance.GetLevelLimitation(3) || 
             clampPos.z <= GameManager.Instance.GetLevelLimitation(2) )
        {
            // Reset the vertical velocity.
            resetVerticalVelocity();
        }


        // Clamp the x and z values.
        clampPos.x = Mathf.Clamp(clampPos.x, GameManager.Instance.GetLevelLimitation(0), GameManager.Instance.GetLevelLimitation(1));
        clampPos.z = Mathf.Clamp(clampPos.z, GameManager.Instance.GetLevelLimitation(2), GameManager.Instance.GetLevelLimitation(3));

        // Set the current position to the clamped vector.
        myTransform.position = clampPos;
    }



    LevelManager.QuarterArea getCurrentQuarter()
    {
        Vector3 Pos = myTransform.position;

        // Check if target is in the UpperRight Quarter.
        if ( Pos.x > 0.0f && Pos.z > 0.0f )
            return LevelManager.QuarterArea.UpperRight;
        // Check if target is in the UpperLeft Quarter.
        else if ( Pos.x < 0.0f && Pos.z > 0.0f )
            return LevelManager.QuarterArea.UpperLeft;
        // Check if target is in the LowerLeft Quarter.
        else if ( Pos.x < 0.0f && Pos.z < 0.0f )
            return LevelManager.QuarterArea.LowerLeft;
        // Check if target is in the LowerRight Quarter.
        else if (Pos.x > 0.0f && Pos.z < 0.0f)
            return LevelManager.QuarterArea.LowerRight;
        // Center lines.
        else
            return LevelManager.QuarterArea.Unknown;
    }

    protected override void onGamePausedHandler( bool pause )
    {
        base.onGamePausedHandler( pause );
        _myAnim.speed = pause ? 0 : 1.0f;

        if ( pause )
        {
            if ( _invincibleTask != null )
            {
                //if ( _invincibleTask.IsRunning )
                _invincibleTask.pause();
            }

            if ( _superFireRateTask != null )
                _superFireRateTask.pause();

            if ( _reviveTask != null )
            {
                _reviveTask.pause();
            }
        }
        else
        {
            if ( _invincibleTask != null )
            {
                //if ( _invincibleTask.IsPaused )
                _invincibleTask.unPause();
            } 

            if ( _superFireRateTask != null )
                _superFireRateTask.unPause();

            if ( _reviveTask != null )
                _reviveTask.unPause();
        }   
    }

    protected override void OnTriggerEnter( Collider other )
    {
        if ( _isInvincible ) return;

        base.OnTriggerEnter( other );

        if ( other.tag == "Enemy" )
        {
            destroy();
        }
    }

    protected override void applyDamage( float damage )
    {
        if ( _isInvincible ) return;

        base.applyDamage( damage );
        destroy();
    }

    protected override void destroy()
    {
        base.destroy();

        GameManager.Instance.OnPlayerDestroyed();

        Instantiate( ragdoll, myTransform.position, myTransform.rotation );
        gameObject.SetActive( false );
        reset();

        _isDead = true;
    }

    public override void reset()
    {
        base.reset();
        myTransform.position = new Vector3(  0, 0, -40 );
        resetInnerRotation();

        _verticalVal = _horizontalVal = 0.0f;

        // Set move values.
        setDefualtMoveProperties();
        setMoveAccel( 10.0f );
        setMoveSpeed( 110.0f );

        ResetInvincibleTask();
        ResetSuperFireRateTask();

        _superGun.ResetAmmo();

        //setInnerColor( Color.white );
    }

    
    void ResetInvincibleTask()
    {
        _itemGOs[0].SetActive( false );

        if ( _invincibleTask != null )
        {
            _invincibleTask.kill();
            _invincibleTask = null;
        }

        _isInvincible = false;
    }

    void ResetSuperFireRateTask()
    {
        //_itemGOs[0].SetActive( false );

        if ( _superFireRateTask != null )
        {
            _superFireRateTask.kill();
            _superFireRateTask = null;
        }

        defaultGun.setFireRate( 0.2f );
        _superGun.setFireRate( 0.15f );
    }

    void ResetReviveTask()
    {
        if ( _reviveTask != null )
        {
            _reviveTask.kill();
            _reviveTask = null;
        }

        _isDead = false;
    }

    //public void setInvincible( float duration )
    //{
    //    _isInvincible = true;
    //    _invincibleDuration = duration;

    //    if ( _invincibleTask != null )
    //    {
    //        _invincibleTask.kill();
    //        _invincibleTask = null;
    //    }

    //    _invincibleTask = new CoroutineTask( invincibleEffect(), true );
    //}

    IEnumerator SuperFireRate( int count )
    {
        defaultGun.setFireRate( 0.1f );
        _superGun.setFireRate( 0.1f );

        yield return new WaitForSeconds( count );

        defaultGun.setFireRate( 0.2f );
        _superGun.setFireRate( 0.15f );
    }


    IEnumerator invincibleEffect( int count )
    { 
        _itemGOs[0].SetActive( true );

        while ( count > 0 )
        {
            //setInnerColor( count % 2 == 0 ? Color.red : Color.white );

            if ( count < 10 )
                _itemGOs[0].SetActive( !_itemGOs[0].activeSelf );

            count--;
            yield return new WaitForSeconds( 0.3f );
        }

        //setInnerColor( Color.white );
        _isInvincible = false;

        _itemGOs[0].SetActive( false );
    }

    public void respawn()
    {
        if (  _reviveTask != null )
        {
            _reviveTask.kill();
            _reviveTask = null;
        }
        
        _reviveTask = new CoroutineTask( startRespawn(), true );
    }

    IEnumerator startRespawn()
    {
        yield return new WaitForSeconds( 2.0f );

        while ( GameManager.Instance.State == GameState.Paused )
            yield return new WaitForEndOfFrame();

        gameObject.SetActive( true );

        _isDead = false;
        _isInvincible = true;
        _invincibleTask = new CoroutineTask( invincibleEffect( 10 ), true );
    }
    

    public void Activate( bool active )
    {
        setActive( active );
        gameObject.SetActive( active );
    }



    public void CancelAllTasks()
    {
        ResetInvincibleTask();
        ResetSuperFireRateTask();
        ResetReviveTask();
    }



    public void ApplyPowerUp( int id )
    {
        switch ( id )
        {
            case 0:
                ResetInvincibleTask();
                _isInvincible = true;
                _invincibleTask = new CoroutineTask( invincibleEffect( 30 ), true );
                break;
            case 1:
                ResetSuperFireRateTask();
                _superFireRateTask = new CoroutineTask( SuperFireRate( 20 ), true );
                break;
            case 2:
                _superGun.addAmmo( 500 );
                break;
            case 3:
                GameManager.Instance.UpdatePlayerLifes( 1 );
                break;
        }
    }
}
