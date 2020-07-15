using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MANA3D.UI.Tween;

namespace MANA3D.UI.Tween
{
    public class UITweenManager
    {
        List<UITween> _tweens;
        List<UITween> _toBeAdded;
        List<UITween> _toBeRemoved;

        public void DEBUG_LOG()
        {
            Debug.Log( "Total: " + _tweens.Count + "      ToBeAdded: " + _toBeAdded.Count + "      ToBeRemoved: " + _toBeRemoved.Count );
        }


        public UITweenManager()
        {
            _tweens = new List<UITween>();
            _toBeAdded = new List<UITween>();
            _toBeRemoved = new List<UITween>();
        }

        public void StartAll()
        {
            foreach ( var tween in _tweens )
                tween.Start();
        }

        public void RemoveAll()
        {
            while ( _tweens.Count > 0 )
                _tweens.RemoveAt( 0 );

            _tweens.Clear();
        }

        public void Update()
        {
            foreach ( var tween in _tweens )
                tween.Update();

            CheckToBeAddedList();
            CheckToBeRemovedList();
        }


        public UITweenAlpha AddUITweenAlpha( UIMenu menu, string name, 
                                             float from, float to, 
                                             float duration = 1, float delay = 0, 
                                             System.Action onComplete = null,
                                             bool removeOnComplete = true,
                                             Action startAction = Action.Show, Action endAction = Action.None )
        {
            GameObject go;
            if ( menu.Components.TryGetValue( name, out go ) )
            {
                Graphic graphic = go.GetComponent<Graphic>();
                UITweenAlpha tween = new UITweenAlpha( graphic, from, to, duration, delay, onComplete, menu, this, removeOnComplete, startAction, endAction );
                _toBeAdded.Add( tween );
                return tween;
            }

            return null;
        }

        public UITweenFill AddUITweenFill( UIMenu menu, string name, 
                                           float from, float to, 
                                           float duration = 1, float delay = 0, 
                                           System.Action onComplete = null,
                                           bool removeOnComplete = true, 
                                           Action startAction = Action.Show, Action endAction = Action.None )
        {
            GameObject go;
            if ( menu.Components.TryGetValue( name, out go ) )
            {
                Image graphic = go.GetComponent<Image>();
                UITweenFill tween = new UITweenFill( graphic, from, to, duration, delay, onComplete, menu, this, removeOnComplete, startAction, endAction );
                _toBeAdded.Add( tween );
                return tween;
            }

            return null;
        }

        public List<UITweenAlpha> AddUITweenAlphaToChildren( UIMenu menu, string name, 
                                                             float from, float to, 
                                                             float duration = 1, float delay = 0, 
                                                             System.Action onComplete = null,
                                                             bool removeOnComplete = true,
                                                             Action startAction = Action.Show, Action endAction = Action.None )
        {
            List<UITweenAlpha> tweens = new List<UITweenAlpha>();
            GameObject go;
            if ( menu.Components.TryGetValue( name, out go ) )
            {
                Transform trans = go.transform;
                for ( int i = 0; i < trans.childCount; i++ )
                {
                    Graphic graphic = trans.GetChild(i).gameObject.GetComponent<Graphic>();
                    if ( graphic )
                    {
                        //UITweenAlpha tween = new UITweenAlpha( graphic, from, to, duration, delay, onComplete, null, this, removeOnComplete, startAction, endAction );
                        UITweenAlpha tween = new UITweenAlpha( graphic, from, to, duration, delay, ( i == trans.childCount - 1 ) ? onComplete : null, null, this, removeOnComplete, startAction, endAction );
                        tween.targetGO = trans.GetChild(i).gameObject;
                        _toBeAdded.Add( tween );
                        tweens.Add( tween );
                    }
                }
                
                return tweens;
            }

            return null;
        }

        public UITweenPosition AddUITweenPosition( UIMenu menu, string name,
                                                   UITween.Type type,
                                                   Vector3 from, Vector3 to, 
                                                   float duration = 1, float delay = 0, 
                                                   System.Action onComplete = null,
                                                   bool removeOnComplete = true,
                                                   Action startAction = Action.Show, Action endAction = Action.None )
        {
            GameObject go;
            if ( menu.Components.TryGetValue( name, out go ) )
            {
                RectTransform rect = go.GetComponent<RectTransform>();
                UITweenPosition tween = new UITweenPosition( rect, type, from, to, duration, delay, onComplete, menu, this, removeOnComplete, startAction, endAction );
                _toBeAdded.Add( tween );
                return tween;
            }

            return null;
        }

        public UITweenScale AddUITweenScale( UIMenu menu, string name,
                                             UITween.Type type,
                                             Vector3 from, Vector3 to, 
                                             float duration = 1, float delay = 0, 
                                             System.Action onComplete = null,
                                             bool removeOnComplete = true, 
                                             Action startAction = Action.Show, Action endAction = Action.None )
        {
            GameObject go;
            if ( menu.Components.TryGetValue( name, out go ) )
            {
                RectTransform rect = go.GetComponent<RectTransform>();
                UITweenScale tween = new UITweenScale( rect, type, from, to, duration, delay, onComplete, menu, this, removeOnComplete,startAction, endAction );
                _toBeAdded.Add( tween );
                return tween;
            }

            return null;
        }


        void CheckToBeAddedList()
        {
            if ( _toBeAdded.Count > 0 )
            {
                foreach ( var tween in _toBeAdded )
                    tween.Start();

                _tweens.AddRange( _toBeAdded );
                _toBeAdded.Clear();
            }
        }

        void CheckToBeRemovedList()
        {
            while ( _toBeRemoved.Count > 0 )
            {
                var tween = _toBeRemoved[0];
                _toBeRemoved.RemoveAt( 0 );
                _tweens.Remove( tween );
                tween = null;
            }
        }

        public void RemoveTween( UITween tween )
        {
            _toBeRemoved.Add( tween );
        }
    }
}