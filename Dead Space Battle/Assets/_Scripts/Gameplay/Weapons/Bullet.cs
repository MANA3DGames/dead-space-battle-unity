using UnityEngine;
using System.Collections;

public class Bullet : SpaceElement 
{
    public GameObject plastPrefab;

    public string ignoreTag;// { set; get; }
    public float strength;// { set; get; }
    public float speed;// { set; get; }

    Transform _myTransform;

    float _health;


    protected override void OnEnable()
    {
        base.OnEnable();

        _health = strength;
    }

    void Awake()
    {
        _myTransform = transform;
    }

    void Start()
    {
        _myTransform = transform;
    }

	void Update()
    {
        if ( _isPaused ) return;

        _myTransform.Translate( ( transform.forward + new Vector3(0, 0, speed) ) * Time.deltaTime);
	}

    protected override void OnTriggerEnter( Collider other )
    {
        if (other.tag == "Cleaner")
        {
            clean();
            return;
        }
            

        if ( other.tag != ignoreTag )
            other.SendMessage( "applyDamage", strength, SendMessageOptions.DontRequireReceiver );
    }

    protected override void applyDamage( float damage )
    {
        base.applyDamage( damage );

        _health -= strength;

        if ( _health <= 0 )
        {
            Instantiate( plastPrefab, _myTransform.position, _myTransform.rotation );
            clean();
        }
    }

    void clean()
    {
        SendMessage("recycle");
    }
}
