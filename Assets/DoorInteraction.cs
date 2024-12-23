using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Collider doorBlocker;  // Collider yang menghalangi pintu
    private bool isUnlocked = false;

    private void Start()
    {
        if (doorBlocker != null)
        {
            doorBlocker.enabled = true;  // Pintu terkunci awalnya
        }
    }

    public void UnlockDoor()
    {
        isUnlocked = true;
        if (doorBlocker != null)
        {
            doorBlocker.enabled = false;  // Nonaktifkan blocker saat pintu terbuka
        }
        Debug.Log("Pintu terbuka!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wizard") && !isUnlocked)
        {
            Debug.Log("Pintu terkunci. Selesaikan quiz!");
        }
    }
}
