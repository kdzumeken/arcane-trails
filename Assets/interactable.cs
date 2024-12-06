using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string itemName; // Nama item

    private void OnMouseDown()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            inventory.AddItem(itemName);
            Destroy(gameObject); // Hapus objek setelah diambil
            Debug.Log($"Item '{itemName}' ditambahkan ke inventory dengan klik.");
        }
    }
}
