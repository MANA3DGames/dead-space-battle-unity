using UnityEngine;
using System.Collections;

namespace MANA3D.UI.Tween
{
    public class Vector3Values
    {
        public float min;
        public float max;
        public float to;
        public float interval;
    }

    public enum Action { None, Show, Hide }

    public class UITween 
    {
        public enum Type
        {
            Absolute,
            Relative
        }

        protected UIMenu menu;

        protected bool isDone;
        protected bool canStart;

        protected float duration;
        protected float interval;
        protected float startTime;
        protected float delay;

        protected System.Action onComplete;

        protected bool removeOnComplete;
        public System.Action RemoveOnComplete;

        protected string componentName;

        public GameObject targetGO;

        protected Action startAction;
        protected Action endAction;


        protected bool StopUpdate
        {
            get
            {
                return ( !canStart || 
                         isDone ||
                         Time.time < startTime + delay );
            }
        }



        public UITween( string componentName, 
                        UIMenu menu = null, UITweenManager manager = null,
                        Action startAction = Action.None, Action endAction = Action.None,
                        bool removeOnComplete = true )
        {
            this.menu = menu;
            this.componentName = componentName;
            this.startAction = startAction;
            this.endAction = endAction;

            if ( removeOnComplete )
                RemoveOnComplete = () => { manager.RemoveTween( this ); };
        }

        public virtual void Start( float after = 0 )
        {
            if ( menu != null )
            {
                menu.ShowComponent( componentName, startAction == Action.Show );
            }
            else if ( targetGO && startAction == Action.Show )
                targetGO.SetActive( true );

            canStart = true;
            delay += after;
            startTime = Time.time;
        }

        public virtual void Update()
        {
        }


        protected void OnDone()
        {
            if (onComplete != null)
                onComplete.Invoke();

            isDone = true;

            if (RemoveOnComplete != null)
                RemoveOnComplete.Invoke();
        }
    }
}

