using UnityEngine;

public class CollectBall : MonoBehaviour
{
    public string ballName;  // Nama bola (rollingball1, rollingball2, rollingball3)

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wizard"))  // Pastikan hanya wizard yang bisa mengumpulkan bola
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.AddItem(ballName, gameObject);
                gameObject.SetActive(false);  // Sembunyikan bola setelah dikumpulkan
                Debug.Log(ballName + " collected!");
            }
        }
    }
}
