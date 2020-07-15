using UnityEngine;

namespace MANA3D.TTween
{
    public class TTweenScale : TTweenVector3
    {
        Transform _transform;


        public override void SetValues(  Type type,
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
            _transform.localScale = fromVector3;
        }

        public override void Update()
        {
            if ( StopUpdate ) return;

            float x, y, z;
            x = y = z = 0;

            if ( xVal.interval != 0 )
                x = _transform.localScale.x + ( interval );// * xVal.factor );
            if ( yVal.interval != 0 )
                y = _transform.localScale.y + ( interval );// * yVal.factor );
            if ( zVal.interval != 0 )
                z = _transform.localScale.z + ( interval );// * zVal.factor );

            x = Mathf.Clamp( x, xVal.min, xVal.max );
            y = Mathf.Clamp( y, yVal.min, yVal.max );
            z = Mathf.Clamp( z, zVal.min, zVal.max );

            _transform.localScale = new Vector3(x, y, z);

            if (x == xVal.to && y == yVal.to && z == zVal.to)
                OnDone();
        }
    }
}