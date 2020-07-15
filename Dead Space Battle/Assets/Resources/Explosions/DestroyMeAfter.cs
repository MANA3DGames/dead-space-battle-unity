using UnityEngine;

public class DestroyMeAfter : MonoBehaviour 
{
    public int ID;
    public float timeOut;

    void Awake()
    {
        GameManager.Instance.OnCreateObjectID( ID, gameObject );
        Destroy( gameObject, timeOut );
    }

    void OnDestroy()
    {
        GameManager.Instance.OnDestroyObjectID( ID, gameObject );
    }
}
