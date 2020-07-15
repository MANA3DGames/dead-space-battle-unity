using UnityEngine;
using System.Collections;

public class Spaceship : SpaceElement 
{
    public bool _isActivated = true;                // Indicates that this spaceship is active and running.

    
    private float _vVelocity = 0;                   // Current vertical velocity.
    private float _hVelocity = 0;                   // Current horizontal velocity.
    protected float vVelocity { get { return _vVelocity; } }
    protected float hVelocity { get { return _hVelocity; } }

    private float _moveAccel;                       // Move acceleration.
    private float _moveDamping;                     // Move de-acceleration (damping factor).
    private float _moveAccelLimit;


    private float _rotationAngle;                   // Current rotation angle around the z-axis.
    private float _rotationAccel;                   // Rotation acceleration.
    private float _rotationDampening;               // Rotation de-acceleration.
    private float _rotationLimit;                   // Limitation of the local rotation around z-axis.

    private float _lookAtSpeed;

    private Transform _innerBody;                   // cache inner part of the ship which will be rotated.

    protected Transform myTransform;               // cache transform component.

    protected BasicGun defaultGun;






    /// <summary>
    /// Caches some components.
    /// </summary>
    protected virtual void Start()
    {
        //  Cache transform component.
        myTransform = transform;

        // Cache the inner part of the spaceship.
        _innerBody = myTransform.GetChild(0);

        // Set defualt Movement values.
        setDefualtMoveProperties();
    }


    protected virtual void setDefualtMoveProperties()
    {
        _moveAccel = 5.0f;                // Move acceleration.
        _moveDamping = 0.95f;             // Move de-acceleration (damping factor).
        _moveAccelLimit = 40;

        _rotationAngle = 0;               // Current rotation angle around the z-axis.
        _rotationAccel = 100;             // Rotation acceleration.
        _rotationDampening = 0.985f;      // Rotation de-acceleration.
        _rotationLimit = 40;              // Limitation of the local rotation around z-axis.
    }


    /// <summary>
    /// Moves spaceship vertically and horizontally.
    /// </summary>
    /// <param name="hVal">Value of the horizontal input</param>
    /// <param name="vVal">Value of the vertical input</param>
    protected void move( float hVal, float vVal )
    {
        // Check if there is a (+) vertical input.
        if ( vVal > 0.0f && _vVelocity < _moveAccelLimit )
            // Apply (+) vertical acceleration.
            _vVelocity += _moveAccel;
        // Check if there is a (-) vertical input.
        else if ( vVal < 0.0f && _vVelocity > -_moveAccelLimit )
            // Apply (-) vertical acceleration.
            _vVelocity -= _moveAccel;
        // There is no vertical input.
        else
            // Apply vertical de-acceleration.
            _vVelocity *= _moveDamping;


        // Check if there is a (+) horizontal input.
        if (hVal > 0.0f && _hVelocity < _moveAccelLimit)
        {
            // Apply (+) horizontal acceleration.
            _hVelocity += _moveAccel;
            // Apply rotation to the right.
            _rotationAngle -= _rotationAccel * Time.deltaTime;
        }
        // Check if there is a (-) horizontal input.
        else if (hVal < 0.0f && _hVelocity > -_moveAccelLimit)
        {
            // Apply (-) horizontal acceleration.
            _hVelocity -= _moveAccel;
            // Apply rotation to the left.
            _rotationAngle += _rotationAccel * Time.deltaTime;
        }
        // There is no horizontal input.
        else
        {
            // Apply horizontal de-acceleration.
            _hVelocity *= _moveDamping;
            // Lerp current rotation to zero.
            _rotationAngle *= _rotationDampening;
        }

        // Move the spaceship with the current velocity.
        myTransform.Translate( _hVelocity * Mathf.Abs( hVal ) * Time.deltaTime, 0.0f, _vVelocity * Mathf.Abs( vVal ) * Time.deltaTime);
    }


    /// <summary>
    /// Zero out the current vertical velocity.
    /// </summary>
    protected void resetVerticalVelocity()
    {
        // Zero out the current vertical velocity.
        _vVelocity = 0.0f;
    }

    /// <summary>
    /// Zero out the current horizontal velocity.
    /// </summary>
    protected void resetHorizontalVelocity()
    {
        // Zero out the current horizontal velocity.
        _hVelocity = 0.0f;
    }


    /// <summary>
    /// Sets the current rotataion angle for the spaceship around the z-axis.
    /// It will angle|-angle.
    /// </summary>
    /// <param name="angle">limitation angle</param>
    protected void setInnerRotationLimit( float angle )
    {
        // Assign angle to _rotationLimit.
        _rotationLimit = angle;
    }


    protected void setMoveSpeed(float speed)
    {
        _moveAccelLimit = speed;
    }

    protected void setMoveAccel(float accel)
    {
        _moveAccel = accel;
    }

    protected void setMoveDamping(float damp)
    {
        _moveDamping = damp;
    }





    
    /// <summary>
    /// Makes the ship transform face a certain position.
    /// </summary>
    /// <param name="targetPosition">vector3 where the ship is going to look</param>
    protected void lookAt( Vector3 targetPosition )
    {
        Quaternion targetRotation = Quaternion.LookRotation( targetPosition - myTransform.position, Vector3.up );
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, targetRotation, _lookAtSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Makes the ship transform face a certain position.
    /// </summary>
    /// <param name="targetPosition">vector3 where the ship is going to look</param>
    /// <param name="speed">rotation speed</param>
    protected void lookAt( Vector3 targetPosition, float speed )
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - myTransform.position, Vector3.up);
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, targetRotation, speed * Time.deltaTime);
    }

    /// <summary>
    /// Makes _myTransform face target Rotation.
    /// </summary>
    /// <param name="newRotation">target rotation</param>
    protected void look( Quaternion newRotation )
    {
        // Set the rotation directly with new Quaternion.
        myTransform.localRotation = Quaternion.Slerp( myTransform.localRotation, newRotation, _lookAtSpeed * Time.deltaTime );
    }

    protected void lookInner( Quaternion newRotation, float speed = 0 )
    {
        float spd = speed == 0 ? _lookAtSpeed : speed;
        _innerBody.rotation = Quaternion.Slerp( _innerBody.rotation, newRotation, spd * Time.deltaTime );
    }

    /// <summary>
    /// Rotate the InnerBody around the Y-axis.
    /// </summary>
    /// <param name="targetPosition">target position, where the innerbody is going to look at</param>
    /// <param name="speed">rotation speed</param>
    protected void lookAtY_Inner_local( Vector3 targetPosition, float speed )
    {
        Quaternion targetRotation = Quaternion.LookRotation( targetPosition - _innerBody.position, Vector3.up );

        _innerBody.localRotation = Quaternion.Slerp( _innerBody.localRotation, targetRotation, speed * Time.deltaTime );
        _innerBody.localRotation = Quaternion.Euler( 0.0f, _innerBody.localEulerAngles.y, 0.0f );
    }

    protected void lookAtY_Inner( Vector3 targetPosition, float speed )
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - _innerBody.position, Vector3.up);

        _innerBody.rotation = Quaternion.Slerp(_innerBody.rotation, targetRotation, speed * Time.deltaTime);
    }

    protected void rotateInner( float x, float y, float z )
    {
        _innerBody.Rotate( x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime );
    }


    /// <summary>
    /// Face target immediatly, no interpolation.
    /// </summary>
    /// <param name="targetPosition">vector3 where the ship is going to look</param>
    protected void lookAt_Immediately( Vector3 targetPosition )
    {
        myTransform.rotation = Quaternion.LookRotation( targetPosition - myTransform.position, Vector3.up );
    }


    protected void resetInnerRotation()
    {
        _innerBody.rotation = Quaternion.identity;
        _innerBody.localRotation = Quaternion.identity;
    }




    /// <summary>
    /// Rotates the spaceship body around the z-axis.
    /// Note: this depends on the current value of _rotationAngle.
    /// </summary>
    protected void rotateZInnerBody()
    {
        // Make sure the rotation doesn't exceed the limitation.
        _rotationAngle = Mathf.Clamp(_rotationAngle, -_rotationLimit, _rotationLimit);

        // Set the rotation directly with Quaternion.
        _innerBody.localRotation = Quaternion.Euler(0, 0, _rotationAngle);
    }


    protected void setLookAtSpeed( float speed )
    {
        _lookAtSpeed = speed;
    }



    float floatVal = 0.1f;
    float _floatingTimer = 0.0f;
    protected void applyFloatEffect()
    {
        if ( Time.time > _floatingTimer + 1.0f )
        {
            floatVal *= -1;
            _floatingTimer = Time.time;
        }

        Vector3 local = new Vector3( _innerBody.localPosition.x, _innerBody.localPosition.y, _innerBody.localPosition.z + floatVal );

        _innerBody.localPosition = Vector3.Lerp(_innerBody.localPosition, local, 6 * Time.deltaTime);
    }


    protected void setInnerColor( Color color )
    {
        if ( _innerBody.GetComponent<Renderer>() )
            _innerBody.GetComponent<Renderer>().material.SetColor( "_Color", color );
    }


    public void setActive( bool active )
    {
        _isActivated = active;
    }
}
