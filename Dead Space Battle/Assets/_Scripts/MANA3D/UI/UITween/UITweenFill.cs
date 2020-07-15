using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MANA3D.UI.Tween
{
    public class UITweenFill : UITween
    {
        Image image;
        //float from;
        float to;
        float max;
        float min;

        public UITweenFill( Image graphic,
                            float from, float to, 
                            float duration = 1, float delay = 0, 
                            System.Action onComplete = null,
                            UIMenu menu = null, UITweenManager manager = null,
                            bool removeOnComplete = true,
                            Action startAction = Action.None, Action endAction = Action.None ) 
                            : base( graphic.gameObject.name, menu, manager, startAction, endAction, removeOnComplete )
        {
            this.image = graphic;
            this.duration = duration;
            this.delay = delay;
            //this.from = from;
            this.to = to;
            this.min = Mathf.Min( from, to );
            this.max = Mathf.Max( from, to );
            this.interval = ( to - from ) / ( duration * 60 );
            this.image.fillAmount = from;
            this.onComplete = onComplete;

            if ( this.image.type != Image.Type.Filled )
            {
                this.image.type = Image.Type.Filled;
                this.image.fillMethod = Image.FillMethod.Vertical;
                this.image.fillOrigin = (int)Image.OriginVertical.Bottom;
            }
        }

        public override void Update()
        {
            if ( StopUpdate ) return;

            float fill = image.fillAmount + interval;
            fill = Mathf.Clamp( fill, min, max );
            image.fillAmount = fill;

            if ( fill == to )
                OnDone();
        }
    }
}
