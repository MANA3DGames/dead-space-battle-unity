using UnityEngine;
using System.Collections;

public class SpaceItem : SpaceElement
{
    public AudioClip pickUpClip;

    public int ID;

    Transform _myTranform;
    AudioSource _audioSource;
    //Renderer _renderer;
    //Collider _collider;


    //protected override void OnEnable()
    //{
    //    base.OnEnable();

    //    if (_renderer)
    //    {
    //        _renderer.enabled = true;
    //        _collider.enabled = true;
    //    }
    //}

    void Start()
    {
        _myTranform = transform;
        _audioSource = GetComponent<AudioSource>();
        //_renderer = GetComponent<Renderer>();
        //_collider = GetComponent<Collider>();
    }


    void Update()
    {
        if ( _isPaused ) return;

        _myTranform.Translate( Vector3.back * 15 * Time.deltaTime );
	}

    protected override void OnTriggerEnter( Collider other )
    {
        if ( other.tag == "Cleaner" )
        {
            clean();
        }
        else if ( other.tag == "Player" )
        {
            other.GetComponent<Player>().ApplyPowerUp( ID );
            _audioSource.PlayOneShot( pickUpClip );

            //_renderer.enabled = false;
            //_collider.enabled = false;

            _myTranform.position = new Vector3( _myTranform.position.x, _myTranform.position.y, -110.0f );

            StartCoroutine( StartClean() );
        }
    }

    IEnumerator StartClean()
    {
        yield return new WaitForSeconds( 3.0f );
        clean();
    }

    void clean()
    {
        GameManager.Instance.OnItemClean();
        SendMessage("recycle");
    }
}
