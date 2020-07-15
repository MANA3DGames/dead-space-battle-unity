using UnityEngine;
using UnityEngine.Networking;

public class PlayerNet : NetworkBehaviour
{
    //public float _moveSpeed = 3.0f;

    //private Vector3 _velocity;

    //public override void OnStartLocalPlayer()
    //{
    //    Debug.Log( " ******************************* OnStartLocalPlayer ******************************* " );
    //    base.OnStartLocalPlayer();

    //    // you can use this function to do things like create camera, audio listeners, etc.
    //    // so things which has to be done only for our player
    //}

    //private void Update()
    //{
    //    // isLocalPlayer is true for the client who "owns" the player object
    //    // we only want input handling for our player
    //    if (!base.isLocalPlayer)
    //        return;

    //    // handle input here...

    //    _velocity = Vector3.zero;

    //    if (Input.GetKey(KeyCode.W))
    //        ++_velocity.z;
    //    if (Input.GetKey(KeyCode.S))
    //        --_velocity.z;
    //    if (Input.GetKey(KeyCode.A))
    //        --_velocity.x;
    //    if (Input.GetKey(KeyCode.D))
    //        ++_velocity.x;

    //    _velocity.Normalize();
    //}

    //private void FixedUpdate()
    //{
    //    // because Local Player Authority is true, the client has to move the player
    //    // only the resulting transform will be sent to the server and to the other clients
    //    if (!base.isLocalPlayer)
    //        return;

    //    // if that flag is not true, we should check that the code runs only on server
    //    // it could be done by checking base.isServer

    //    // transforming and other authoritive stuff here...

    //    transform.position += _velocity * Time.deltaTime * _moveSpeed;
    //}

    int moveX = 0;
	int moveY = 0;


    public override void OnStartClient()
    {

        Debug.Log( "YO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" );
    }


    void Update () 
	{
		//if ( !isLocalPlayer )
  //      {
		//	return;
		//}
		
        //if ( !GetComponent<NetworkIdentity>().isLocalPlayer )
        //    return;

		// input handling for local player only
		int oldMoveX = moveX;
		int oldMoveY = moveY;
		
		moveX = 0;
		moveY = 0;
		
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			moveX -= 1;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			moveX += 1;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			moveY += 1;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			moveY -= 1;
		}
		if ( moveX != oldMoveX || moveY != oldMoveY )
		{
			//CmdMove( "Move", moveX, moveY );
            CmdMove( moveX, moveY );
		}
	}

	[Command]
	public void CmdMove( int dx, int dy )
	{
        // move here
        transform.Translate( dx, 0, dy );
	}
}