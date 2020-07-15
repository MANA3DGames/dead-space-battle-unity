using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MANA3D.UI.Tween
{
    public class UITweenScale : UITweenVector3
    {
        RectTransform _rect;

        public UITweenScale( RectTransform rect, Type type,
                             Vector3 from, Vector3 to,
                             float duration = 1, float delay = 0, 
                             System.Action onComplete = null,
                             UIMenu menu = null, UITweenManager manager = null,
                             bool removeOnComplete = true, 
                             Action startAction = Action.None, Action endAction = Action.None )
                             : base( rect.gameObject.name, type, 
                                     rect.localScale, from, to, 
                                     duration, delay, 
                                     onComplete, menu, manager, 
                                     removeOnComplete, startAction, endAction )
        {
            _rect = rect;
            _rect.localScale = fromVector3;
        }

        public override void Update()
        {
            if ( StopUpdate ) return;

            float x, y, z;
            x = y = z = 0;

            if ( xVal.interval != 0 )
                x = _rect.localScale.x + ( interval );// * xVal.factor );
            if ( yVal.interval != 0 )
                y = _rect.localScale.y + ( interval );// * yVal.factor );
            if ( zVal.interval != 0 )
                z = _rect.localScale.z + ( interval );// * zVal.factor );

            x = Mathf.Clamp( x, xVal.min, xVal.max );
            y = Mathf.Clamp( y, yVal.min, yVal.max );
            z = Mathf.Clamp( z, zVal.min, zVal.max );

            _rect.localScale = new Vector3( x, y, z );

            if ( x == xVal.to && y == yVal.to && z == zVal.to )
                OnDone();
        }
    }
}
