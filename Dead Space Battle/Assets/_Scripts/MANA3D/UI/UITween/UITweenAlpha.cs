using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MANA3D.UI.Tween
{
    public class UITweenAlpha : UITween
    {
        Graphic graphic;
        //float from;
        float to;
        float max;
        float min;

        public UITweenAlpha( Graphic graphic, 
                             float from, float to, 
                             float duration = 1, float delay = 0, 
                             System.Action onComplete = null,
                             UIMenu menu = null, UITweenManager manager = null,
                             bool removeOnComplete = true, 
                             Action startAction = Action.None, Action endAction = Action.None ) 
                            : base( graphic.gameObject.name, menu, manager, startAction, endAction, removeOnComplete )
        {
            this.graphic = graphic;
            this.duration = duration;
            this.delay = delay;
            //this.from = from;
            this.to = to;
            this.min = Mathf.Min( from, to );
            this.max = Mathf.Max( from, to );
            this.interval = ( to - from ) / ( duration * 60 );
            this.graphic.color = new Color(this.graphic.color.r, this.graphic.color.g, this.graphic.color.b, from );
            this.onComplete = onComplete;
        }

        public override void Update()
        {
            if ( StopUpdate ) return;

            float alpha = graphic.color.a + interval;
            alpha = Mathf.Clamp( alpha, min, max );
            graphic.color = new Color( graphic.color.r, graphic.color.g, graphic.color.b, alpha );

            if ( alpha == to )
                OnDone();
        }
    }
}