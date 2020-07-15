using UnityEngine;

namespace MANA3D.TTween
{
    public class TTweenPosition : TTweenVector3
    {
        Transform _transform;

        public override void SetValues( Type type,
                                        Vector3 original, Vector3 from, Vector3 to,
                                        float duration = 1, float delay = 0,
                                        System.Action onComplete = null,
                                        Action startAction = Action.None, 
                                        Action endAction = Action.None )
        {
            base.SetValues( type,
                            original, from, to,
                            duration, delay,
                            onComplete, startAction, endAction );

            _transform = transform;
            _transform.position = fromVector3;
        }

        public override void Update()
        {
            if ( StopUpdate ) return;

            float x, y, z;
            x = y = z = 0;

            if ( xVal.interval != 0 )
                x = _transform.position.x + xVal.interval;
            if ( yVal.interval != 0 )
                y = _transform.position.y + yVal.interval;
            if ( zVal.interval != 0 )
                z = _transform.position.z + zVal.interval;

            x = Mathf.Clamp( x, xVal.min, xVal.max );
            y = Mathf.Clamp( y, yVal.min, yVal.max );
            z = Mathf.Clamp( z, zVal.min, zVal.max );

            _transform.position = new Vector3( x, y, z );

            if ( x == xVal.to && 
                 y == yVal.to && 
                 z == zVal.to )
                OnDone();
        }
    }
}
