using UnityEngine;

public class TestSeeThrough : MonoBehaviour
{
    public Transform cube;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if ( Physics.Raycast( ray, out hit ) )
        {
            cube.position = hit.point;
            GetComponent<Renderer>().material.SetVector( "_CursorPos", cube.position );
        }
    }
}
