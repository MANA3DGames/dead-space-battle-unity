using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour
{
    public Transform otherStage;

    Transform _myTransform;

    float _speed;


    void Start()
    {
        _myTransform = transform;
        _speed = 100.0f;
    }

    void Update()
    {
        _myTransform.Translate(0.0f, 0.0f, _speed * Time.deltaTime);

        if ( _myTransform.position.z <= -337.9f )
            _myTransform.position = new Vector3(0.0f, 0.0f, otherStage.position.z + 334.9f);
    }
}
