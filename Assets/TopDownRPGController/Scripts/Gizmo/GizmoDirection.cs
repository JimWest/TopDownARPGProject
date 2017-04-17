using UnityEngine;
using System.Collections;

public class GizmoDirection : MonoBehaviour {
	
    // Draws a frostum that points in the z (forward) direction
	void OnDrawGizmos() {
		
		float fov = 20;
		float max = 2;
		float min = 0.1f;
		
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation * Quaternion.Euler(Vector3.up * 90), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, fov, max, min, 1);
	}
}

