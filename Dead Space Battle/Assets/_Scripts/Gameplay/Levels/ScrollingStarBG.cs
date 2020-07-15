using UnityEngine;
using System.Collections;

public class ScrollingStarBG : SpaceElement
{
    public float scrollSpeed;
	public float XScrollSpeed;
	public float YScrollSpeed;

    Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if ( _isPaused ) return;

		//renderer.material.SetTextureOffset("_MainTex", new Vector2(0, Time.time * scrollSpeed));
        _renderer.material.SetTextureOffset( "_MainTex", new Vector2( Time.time * XScrollSpeed, Time.time * YScrollSpeed ) );
    }
}
