using UnityEngine;
using System.Collections;

public class MovingItem : Spaceship 
{
    public float movingSpeed = 1;
    public float destroyZ = -330;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if ( _isPaused ) return;

        move( 0, movingSpeed );

        if (myTransform.position.z <= destroyZ)
            pickupRandomSpawn();
    }

    void pickupRandomSpawn()
    {
        myTransform.position = new Vector3( Random.Range( (int)(-105), (int)(105) ),
                                            -160,
                                            Random.Range( (int)(1500), (int)(2000) ) );
    }

    public override void reset()
    {
        base.reset();
        pickupRandomSpawn();
    }
}
