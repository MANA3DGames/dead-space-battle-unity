using UnityEngine;
using System.Collections;

public class Shield : SpaceElement
{
    Transform _myTransform;

	// Use this for initialization
	void Start ()
    {
        _myTransform = transform;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_isPaused) return;

        _myTransform.Rotate( 10 * Time.deltaTime, 0, 10 * Time.deltaTime );
    }
}
