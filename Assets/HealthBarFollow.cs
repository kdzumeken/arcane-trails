using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public Transform target; // Transform musuh
    public Vector3 offset = new Vector3(0, 2, 0); // Offset posisi health bar di atas kepala

    void LateUpdate()
    {
        if (target != null)
        {
            // Posisi canvas mengikuti musuh dengan offset
            transform.position = target.position + offset;
        }
    }
}
