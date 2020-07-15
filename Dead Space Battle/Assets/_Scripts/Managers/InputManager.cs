using UnityEngine;
using System.Collections;

public class InputDelay
{
    public float duration;
    public float lastTime;

    public bool CanProceed { get { return Time.time > lastTime + duration; } }
}

public struct JoystickSettings
{
    public int moveJoystickSize;
    public int fireJoystickSize;
}

public class InputManager : MonoBehaviour 
{
    public bool IsActivated { get { return _isActivated; } }
    private bool _isActivated;

    public float RawVerticalVal { get { return _rawVertical; } }
    private float _rawVertical;
    public float RawHorizontalVal { get { return _rawHorizontal; } }
    private float _rawHorizontal;
    public bool IsFiring { get { return _isFiring; } }
    private bool _isFiring;
    public bool IsAiming { get { return _isAiming; } }
    private bool _isAiming;
    public Vector2 LookAt { get { return _lookAt; } }
    private Vector2 _lookAt;

    InputDelay _delay;

    Vector3 _mousePos;
    Transform _aimPointer;
    MANA.UITweenUtil.ColorTweenSprite _colorTween;


    EasyJoystick Move_Joystick;
    EasyJoystick Attack_Joystick;


    public JoystickSettings joystickSettings { get { return _joysticksSettings; } }
    JoystickSettings _joysticksSettings;


    void Start()
    {
        _delay = new InputDelay();
        _delay.duration = 0.2f;
        _delay.lastTime = Time.time;

        Move_Joystick = GameObject.Find( "Move_Joystick" ).GetComponent<EasyJoystick>();
        Move_Joystick.enable = false;
        Attack_Joystick = GameObject.Find( "Attack_Joystick" ).GetComponent<EasyJoystick>();
        Attack_Joystick.enable = false;

        LoadJoysticksSettings();
    }

    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Escape ) )
            GameManager.Instance.MinimizeApp();

        if ( !_isActivated ) return;

        if ( GameManager.Instance.State == GameState.Title )
        {
            CheckTitleInput();
        }
        else if ( GameManager.Instance.State == GameState.Gameplay )
        {
            CheckGameplayInput();
        }
        else if ( GameManager.Instance.State == GameState.Paused )
        {
            CheckPausedInput();
        }

    }


    void CheckTitleInput()
    {
        //if ( Input.anyKeyDown )
        //    GameManager.Instance.OnPressAnyKey();

    }

    void CheckGameplayInput()
    {
        if ( Input.GetKeyDown( KeyCode.Escape ) && _delay.CanProceed )
        {
            GameManager.Instance.PauseGame();
            _delay.lastTime = Time.time;
            return;
        }

        //_isFiring = Input.GetKey( KeyCode.Space ) || Input.GetMouseButton( 0 );
        //_isAiming = Input.GetMouseButton( 1 );

        //if ( _isFiring || _isAiming )//if ( _isAimFiring )
        //{
        //    //// Cancel normal firing.
        //    //_isFiring = false;

        //    Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        //    RaycastHit hit;

        //    if ( Physics.Raycast( ray, out hit ) )
        //    {
        //        _aimPointer.position = hit.point;
        //        _colorTween.blendInAlpha();
        //        _lookAt = new Vector2( _aimPointer.position.x, _aimPointer.position.z );
        //    }
        //}
        //else
        //{
        //    _colorTween.resetSpeed();
        //    _colorTween.blendOutAlpha();

        //    // Look straightforward
        //    _lookAt = new Vector2( 0, 200 );
        //}

        //float vertical = 0, horizontal = 0;
        //// Get mouse movement if the player is not aiming and firing.
        //if (!_isAimFiring)
        //{
        //    //var temp = ( Input.mousePosition - _mousePos ).normalized;
        //    //if ( Mathf.Abs( temp.y ) > 0.7f )
        //    //    vertical = temp.y;
        //    //if ( Mathf.Abs( temp.x ) > 0.7f )
        //    //    horizontal = temp.x;

        //    vertical = Input.GetAxis("Mouse Y");
        //    if (Mathf.Abs(vertical) < 0.5f)
        //        vertical = 0.0f;

        //    horizontal = Input.GetAxis("Mouse X");
        //    if (Mathf.Abs(horizontal) < 0.5f)
        //        horizontal = 0.0f;
        //}

        // Check if there is no value coming from the mouse movement.
        //if (vertical == 0)
        //    vertical = Input.GetAxisRaw("Vertical");
        //if (horizontal == 0)
        //    horizontal = Input.GetAxisRaw("Horizontal");

        //_verticalVal = Mathf.Abs(_rawVertical) > 0.0f ? _rawVertical : Mathf.Lerp(_verticalVal, 0.0f, 5.0f * Time.deltaTime);
        //_horizontalVal = Mathf.Abs(_rawHorizontal) > 0.0f ? _rawHorizontal : Mathf.Lerp(_horizontalVal, 0.0f, 5.0f * Time.deltaTime);
    }

    void CheckPausedInput()
    {
        if ( Input.GetKeyDown( KeyCode.Escape ) && _delay.CanProceed )
        {
            GameManager.Instance.ResumeGame();
            _delay.lastTime = Time.time;
            return;
        }
    }


    public void SetAimPointer( Transform aim )
    {
        _aimPointer = aim;
        _colorTween = _aimPointer.GetComponent<MANA.UITweenUtil.ColorTweenSprite>();
    }

    public void Activate( bool active )
    {
        _isActivated = active;
    }

    public void ResetPlayerControls()
    {
        _isFiring = false;
        //_isAimFiring = false;
        _lookAt = Vector2.zero;
        _rawVertical = _rawHorizontal = 0.0f;
        _colorTween.blendOutAlpha();
    }



    public void EnablePlayerController( bool enable )
    {
        Move_Joystick.enable = enable;
        Attack_Joystick.enable = enable;
    }


    void OnEnable()
    {
		EasyJoystick.On_JoystickMove += On_JoystickMove;
		EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
	}

    void OnDisable()
    {
		EasyJoystick.On_JoystickMove -= On_JoystickMove	;
		EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
	}

    void OnDestroy()
    {
		EasyJoystick.On_JoystickMove -= On_JoystickMove;	
		EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
	}
	
	
	void On_JoystickMoveEnd( MovingJoystick move )
    {
        if ( move.joystickName == "Move_Joystick" )
        {
            _rawVertical = 0.0f;
            _rawHorizontal = 0.0f;
        }

        if ( move.joystickName == "Attack_Joystick" )
        {
            _isFiring = false;
            _isAiming = false;
        }
	}

	void On_JoystickMove( MovingJoystick move )
    {
		if ( move.joystickName == "Move_Joystick" )
        {
            _rawVertical = move.joystickAxis.y;
            _rawHorizontal = move.joystickAxis.x;

            //_verticalVal = Mathf.Abs( _rawVertical ) > 0.0f ? _rawVertical : Mathf.Lerp( _verticalVal, 0.0f, 5.0f * Time.deltaTime );
            //_horizontalVal = Mathf.Abs( _rawHorizontal ) > 0.0f ? _rawHorizontal : Mathf.Lerp( _horizontalVal, 0.0f, 5.0f * Time.deltaTime );
		}

        if ( move.joystickName == "Attack_Joystick" )
        {
            _isFiring = true;
            _isAiming = true;

            if ( _isFiring || _isAiming )
            {
                _lookAt = new Vector2( move.joystickAxis.x * 200.0f, move.joystickAxis.y * 200.0f );
            }
        }
    }


    public void LoadJoysticksSettings()
    {
        _joysticksSettings = new JoystickSettings();
        _joysticksSettings.moveJoystickSize = PlayerPrefs.GetInt( "moveJoystickSize" );
        _joysticksSettings.fireJoystickSize = PlayerPrefs.GetInt( "fireJoystickSize" );
        
        if ( _joysticksSettings.moveJoystickSize != 0 )
        {
            Move_Joystick.ZoneRadius = 75;
            Move_Joystick.TouchSize = 22.5f;
            Move_Joystick.deadZone = 15;
        }
        else
        {
            Move_Joystick.ZoneRadius = 50;
            Move_Joystick.TouchSize = 15;
            Move_Joystick.deadZone = 10;
        }

        if ( _joysticksSettings.fireJoystickSize != 0 )
        {
            Attack_Joystick.ZoneRadius = 75;
            Attack_Joystick.TouchSize = 22.5f;
            Attack_Joystick.deadZone = 0;
        }
        else
        {
            Attack_Joystick.ZoneRadius = 50;
            Attack_Joystick.TouchSize = 15;
            Attack_Joystick.deadZone = 0;
        }
    }

    public void SetFireJoysticksSettings( int id )
    {
        _joysticksSettings.fireJoystickSize = id;
        PlayerPrefs.SetInt( "fireJoystickSize", id );
    }
    public void SetMoveJoysticksSettings( int id )
    {
        _joysticksSettings.moveJoystickSize = id;
        PlayerPrefs.SetInt( "moveJoystickSize", id );
    }

}
