using UnityEngine;
using System.Collections;

public class ImageEffectManager : MonoBehaviour 
{
    public Transform CameraHome;
    public Transform GameplayCamera;


    private float _shakingDuration;
    private float _shakeAmount;
    private float _minXShaking;
    private float _maxXShaking;
    private float _minYShaking;
    private float _maxYShaking;
    private float _minZShaking;
    private float _maxZShaking;

    private const float DECREASE_FACTOR = 1.0f;

    private bool _startShaking;



    void Start()
    {
        if ( !GameplayCamera )
            GameplayCamera = Camera.main.transform;
    }

    void Update()
    {
        if (!_startShaking)
            return;


        if ( _shakingDuration > 0.0f )
        {
            GameplayCamera.localPosition = Random.insideUnitCircle * _shakeAmount;

            float xlimitation = Mathf.Clamp(GameplayCamera.localPosition.x, _minXShaking, _maxXShaking);
            float ylimitation = Mathf.Clamp(GameplayCamera.localPosition.y, _minYShaking, _maxYShaking);
            float zlimitation = Mathf.Clamp(GameplayCamera.localPosition.z, _minZShaking, _maxZShaking);
            GameplayCamera.localPosition = new Vector3(xlimitation, ylimitation, zlimitation);

            _shakingDuration -= Time.deltaTime * DECREASE_FACTOR;
            _shakeAmount -= Time.deltaTime;
        }
        else
        {
            ResetCamera();
        }
    }



    void ResetCamera()
    {
        GameplayCamera.parent = null;

        _shakingDuration = 0.0f;
        _startShaking = false;
    }

    public void ShakeCamera( float shakeAmount, float duration )
    {
        if (!CameraHome)
            CameraHome = GameObject.Find("CameraHome").transform;


        CameraHome.position = GameplayCamera.position;
        CameraHome.rotation = GameplayCamera.rotation;

        GameplayCamera.parent = CameraHome;


        _minXShaking = GameplayCamera.localPosition.x - 3.0f;
        _maxXShaking = GameplayCamera.localPosition.x + 3.0f;

        _minYShaking = GameplayCamera.localPosition.y;
        _maxYShaking = GameplayCamera.localPosition.y + 4.0f;

        _minZShaking = GameplayCamera.localPosition.z - 4.0f;
        _maxZShaking = GameplayCamera.localPosition.z;


        _shakingDuration = duration;
        _shakeAmount = shakeAmount;

        _startShaking = true;
    }

}
