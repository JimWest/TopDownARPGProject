using UnityEngine;
using System.Collections;

public class GizmoBox : MonoBehaviour
{

    public Color gizmoColor = new Color(1, 0, 0, 0.5F);

    BoxCollider _boxCollider;

    void OnEnable()
    {
       
    }

    void OnDrawGizmos()
    {
        // apply rotation
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gizmoColor;

        _boxCollider = GetComponent<BoxCollider>();

        if (_boxCollider)
        {
            Gizmos.DrawCube(_boxCollider.center, _boxCollider.size);
        }
        else
        {
            Gizmos.DrawCube(Vector3.zero, transform.localScale);
        }


    }
}


