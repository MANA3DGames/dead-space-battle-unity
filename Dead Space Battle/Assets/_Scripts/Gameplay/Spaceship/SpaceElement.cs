using UnityEngine;
using System.Collections;

[System.Serializable]
public struct CoreElement
{
    public enum Element
    {
        E1,
        E2,
        E3
    }

    public Element core;
    public float percentage;
}

public class SpaceElement : MonoBehaviour 
{
    public CoreElement[] coreElement;

    protected bool _isPaused;

    protected virtual void OnEnable()
    {
        GameManager.Instance.onGamePaused += onGamePausedHandler;
    }

    protected virtual void OnDisable()
    {
        GameManager.Instance.onGamePaused -= onGamePausedHandler;
    }

    protected virtual void onGamePausedHandler( bool pause )
    {
        //throw new System.NotImplementedException();
        _isPaused = pause;
    }

    public virtual void reset()
    {

    }

    protected virtual void applyDamage( float damage )
    {
        // This is just a prototype so we are not going to use the damage value for now.
    }

    protected virtual void OnTriggerEnter( Collider other )
    {
    }

    protected virtual void destroy()
    {

    }
}
