using UnityEngine;
using System.Collections;

public class PointGUI : Spaceship 
{
    public float movingSpeed = 1;
    public float destroyZ = 105;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if ( _isPaused ) return;

        move( 0, movingSpeed );

        if (myTransform.position.z >= destroyZ)
            SendMessage("recycle");
    }

}
