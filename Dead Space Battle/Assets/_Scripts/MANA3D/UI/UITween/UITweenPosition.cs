using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MANA3D.UI.Tween
{
    public class UITweenPosition : UITweenVector3
    {
        RectTransform _rect;

        public UITweenPosition( RectTransform rect, Type type,
                                Vector2 from, Vector2 to,
                                float duration = 1, float delay = 0, 
                                System.Action onComplete = null,
                                UIMenu menu = null, UITweenManager manager = null,
                                bool removeOnComplete = true,
                                Action startAction = Action.None, Action endAction = Action.None ) 
                                : base( rect.gameObject.name, type, 
                                        rect.localPosition, from, to, 
                                        duration, delay, 
                                        onComplete, menu, manager, 
                                        removeOnComplete, startAction, endAction )
        {
            _rect = rect;
            _rect.anchoredPosition = fromVector3;
        }

        public override void Update()
        {
            if ( StopUpdate ) return;

            float x, y;
            x = y = 0;

            if ( xVal.interval != 0 )
                x = _rect.anchoredPosition.x + ( interval );
            if ( yVal.interval != 0 )
                y = _rect.anchoredPosition.y + ( interval );

            x = Mathf.Clamp( x, xVal.min, xVal.max );
            y = Mathf.Clamp( y, yVal.min, yVal.max );

            _rect.anchoredPosition = new Vector2( x, y );

            if ( x == xVal.to && y == yVal.to )
                OnDone();
        }
    }
}
