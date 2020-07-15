using UnityEngine;
using UnityEngine.UI;
using MANA3D.Utilities.Math;


namespace MANA.UITweenUtil
{
    public class UITween : MonoBehaviour
    {
        public enum Mode { Once, PingPong }
        public Mode mode;

        public bool destroyWhenDone = true;

        protected void destroyMe()
        {
            Destroy( this );
        }
    }

    public class ColorTween : UITween
    {
        public System.Func<VoidDelegate> callBack;
        public Color targetColor;
        public bool blendAlphaIn;
        public bool blendAlphaOut;

        protected float _speed = 1.0f;
        Graphic _graphic;

        protected virtual void Start()
        {
            _graphic = GetComponent<Graphic>();
            
            if ( blendAlphaIn ) 
                blendInAlpha();
            else if ( blendAlphaOut ) 
                blendOutAlpha();

            _speed = 1.0f;
        }

        protected virtual void Update()
        {
            if ( _graphic.color == targetColor )
            {
                if ( callBack != null )
                    callBack.Invoke();

                if ( destroyWhenDone )
                    destroyMe();
            }

            _graphic.color = Color.Lerp( _graphic.color, targetColor, _speed * Time.deltaTime );

            if ( MathOperation.distance( _graphic.color.r, targetColor.r ) < 0.15f &&
                 MathOperation.distance( _graphic.color.g, targetColor.g ) < 0.15f &&
                 MathOperation.distance( _graphic.color.b, targetColor.b ) < 0.15f &&
                 MathOperation.distance( _graphic.color.a, targetColor.a ) < 0.15f )
                _speed = Mathf.Lerp( _speed, 50, Time.deltaTime );
        }

        public virtual void blendInAlpha()
        {
            targetColor = new Color( _graphic.color.r, _graphic.color.g, _graphic.color.b, 1 );
        }
        public virtual void blendOutAlpha()
        {
            targetColor = new Color( _graphic.color.r, _graphic.color.g, _graphic.color.b, 0 );
        }
    }

    public class ColorTweenSprite : ColorTween
    {
        SpriteRenderer _graphic;

        protected override void Start()
        {
            _graphic = GetComponent<SpriteRenderer>();
            
            if ( blendAlphaIn ) 
                blendInAlpha();
            else if ( blendAlphaOut ) 
                blendOutAlpha();

            _speed = 5.0f;
        }

        protected override void Update()
        {
            if ( _graphic.color == targetColor )
            {
                if ( callBack != null )
                    callBack.Invoke();

                if ( destroyWhenDone )
                    destroyMe();
            }

            _graphic.color = Color.Lerp( _graphic.color, targetColor, _speed * Time.deltaTime );

            if ( MathOperation.distance( _graphic.color.r, targetColor.r ) < 0.15f &&
                 MathOperation.distance( _graphic.color.g, targetColor.g ) < 0.15f &&
                 MathOperation.distance( _graphic.color.b, targetColor.b ) < 0.15f &&
                 MathOperation.distance( _graphic.color.a, targetColor.a ) < 0.15f )
                _speed = Mathf.Lerp( _speed, 50, Time.deltaTime );
        }

        public override void blendInAlpha()
        {
            targetColor = new Color( _graphic.color.r, _graphic.color.g, _graphic.color.b, 1 );
        }
        public override void blendOutAlpha()
        {
            targetColor = new Color( _graphic.color.r, _graphic.color.g, _graphic.color.b, 0 );
        }

        public void resetSpeed()
        {
            _speed = 5.0f;
        }
    }

    public class ScaleTween : UITween
    {
        public System.Func<VoidDelegate> callBack;
        public Vector3 endScale;
        Vector3 _startScale;
        Vector3 _currentTargetScale;

        System.Func<VoidDelegate> mainFuc;
        Transform _transform;
        float _speed = 1.0f;


        void Start()
        {
            _transform = transform;
            _startScale = _transform.localScale;

            _speed = 1.0f;

            switch ( mode )
            {
                case Mode.Once:
                    mainFuc = once;
                    break;
                case Mode.PingPong:
                    mainFuc = pingPong;
                    break;
            }

            _currentTargetScale = endScale;
        }

        void Update()
        {
            mainFuc.Invoke();
        }

        VoidDelegate once()
        {
            if ( _transform.localScale == _currentTargetScale )
            {
                if ( callBack != null )
                    callBack.Invoke();

                if ( destroyWhenDone )
                    destroyMe();
            }

            _transform.localScale = Vector3.Lerp( _transform.localScale, _currentTargetScale, _speed * Time.deltaTime );

            checkToIncreaseSpeed();

            return null;
        }

        VoidDelegate pingPong()
        {
            if ( _transform.localScale == _currentTargetScale )
            {
                _currentTargetScale = _currentTargetScale == _startScale ? endScale : _startScale;
                _speed = 1;
            }

            _transform.localScale = Vector3.Lerp( _transform.localScale, _currentTargetScale, _speed * Time.deltaTime );

            checkToIncreaseSpeed();

            return null;
        }

        void checkToIncreaseSpeed()
        {
            if ( MathOperation.distance( _transform.localScale.x, _currentTargetScale.x ) < 0.1f &&
                 MathOperation.distance( _transform.localScale.y, _currentTargetScale.y ) < 0.1f &&
                 MathOperation.distance( _transform.localScale.z, _currentTargetScale.z ) < 0.1f )
                _speed = Mathf.Lerp( _speed, 10, Time.deltaTime );
        }
    }
}
