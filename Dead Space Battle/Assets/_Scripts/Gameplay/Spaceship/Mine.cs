using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Mine : Enemy 
{
    public GameObject explosionPrefab;



    public enum MineApproach
    {
        RandomDirect,
        Directed,
        DirectShooter,
        RandomShooter
    }

    public enum ExpolsionShape
    {
        SphereExp,
        timerExplosion,
        HorExp,
        VerExp,
        TirLines,
        Hurricane,
    }

    public struct MineInfo
    {
        public Rank rank;
        public ExpolsionShape explosionShape;
        public MineApproach approach;
        public GameObject explosionGO;

        public List<string> explosionTags;
    }


    private MineInfo info;





    protected override void Start()
    {
        base.Start();

        info = new MineInfo();
        info.rank = Rank.Mine;
        info.explosionShape = ExpolsionShape.SphereExp;
        info.approach = MineApproach.RandomShooter;
        info.explosionTags = new List<string>();
        info.explosionTags.Add( "Player" );


        switch (info.approach)
        {
            case MineApproach.RandomDirect:
                lookAt_Immediately(findRandomPoint());
                break;
            case MineApproach.DirectShooter:
                defaultGun = new BasicGun(100000, 100000, 10, 0.5f, 200, gunSFX, gunTransform, gunLight, gunBullet, gunSpawnPoint, name + "_Bullets", tag);
                lookAt_Immediately(findRandomPoint());
                break;
            case MineApproach.RandomShooter:
                defaultGun = new BasicGun(100000, 100000, 10, 0.2f, 100, gunSFX, gunTransform, gunLight, gunBullet, gunSpawnPoint, name + "_Bullets", tag);
                lookAt_Immediately(findRandomPoint());
                break;
            case MineApproach.Directed:
                lookAt_Immediately(findRandomPointNearToTarget());
                break;
           
        }
        
    }

    protected override void Update()
    {
        switch ( info.approach )
        {
            case MineApproach.Directed:
            case MineApproach.RandomDirect:
                move(0.0f, 1.0f);
                break;
            case MineApproach.DirectShooter:
                lookAtTarget( 4.0f );
                fire();
                move(0.0f, 0.1f);
                break;
            case MineApproach.RandomShooter:
                lookRandomly( 25.0f );
                fire();
                move(0.0f, 0.1f);
                break;
        }
    }







    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    // Update Effects.
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    void updateFlasher()
    {

    }

    void rotate()
    {

    }

    void lookAtTarget(float lookSpeed)
    {
        lookAtY_Inner(Player.Instance.transform.position, lookSpeed);
    }

    void lookRandomly( float lookSpeed )
    {
        lookAtY_Inner(new Vector3( Random.Range(GameManager.Instance.GetLevelLimitation(0) - 100, GameManager.Instance.GetLevelLimitation(1) + 100),
                                    0.0f,
                                    Random.Range(GameManager.Instance.GetLevelLimitation(2) - 100, GameManager.Instance.GetLevelLimitation(3) + 100)), lookSpeed);
    }


    





    Vector3 findRandomPoint()
    {
        Vector3 target = Player.Instance.transform.position;

        target = new Vector3(Random.Range((int)GameManager.Instance.GetLevelLimitation(0), (int)GameManager.Instance.GetLevelLimitation(1)), 0.0f, -100);

        return target;
    }

    Vector3 findRandomPointNearToTarget()
    {
        Vector3 target = Player.Instance.transform.position;
        target = new Vector3( target.x + Random.Range( -5, 5 ), target.y, target.z );
        return target;
    }

    

    void explode()
    {
        Instantiate( explosionPrefab, myTransform.position, myTransform.rotation );
        GameManager.Instance.ShakeCamera( 3.0f, 1.0f );
        Destroy( gameObject );
    }



    protected override void OnTriggerEnter( Collider other )
    {
        foreach ( string item in info.explosionTags )
        {
            if ( other.tag == item )
            {
                if (defaultGun != null)
                    defaultGun.disableGun();
                explode();
                return;
            }
        }
        
    }

}
