using UnityEngine;

public class FirePointGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
