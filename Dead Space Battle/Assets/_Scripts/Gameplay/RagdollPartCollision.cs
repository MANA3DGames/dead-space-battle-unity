using UnityEngine;
using System.Collections;

public class RagdollPartCollision : MonoBehaviour
{
    public GameObject explosionPrefab;

    bool _canDamage;

    IEnumerator Start()
    {
        _canDamage = false;
        yield return new WaitForSeconds( 0.1f );
        _canDamage = true;
    }

    void applyDamage( float damage )
    {
        if ( _canDamage )
        {
            Instantiate( explosionPrefab, transform.position, transform.rotation );
            gameObject.SetActive( false );
        }
    }
}
