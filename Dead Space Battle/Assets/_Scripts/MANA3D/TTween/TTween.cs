using UnityEngine;
using System.Collections;

namespace MANA3D.TTween
{
    public enum Action { None, Show, Hide }

    public class TTween : MonoBehaviour
    {
        public enum Type
        {
            Absolute,
            Relative
        }

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

        protected GameObject targetGO;

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



        public virtual void SetValues( Action startAction = Action.None, Action endAction = Action.None )
        {
            this.startAction = startAction;
            this.endAction = endAction;

            if ( startAction == Action.Show )
            {
                if ( targetGO )
                    targetGO.SetActive( true );

                gameObject.SetActive( true );
            }

            canStart = true;
            startTime = Time.time;
        }

        //public virtual void Start()
        //{
        //    Debug.Log( "aaaaaaaaaaaaaaa" );
        //    if ( targetGO && startAction == Action.Show )
        //        targetGO.SetActive( true );

        //    canStart = true;
        //    startTime = Time.time;
        //}

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