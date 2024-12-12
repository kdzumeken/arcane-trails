using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string itemName; // Nama item
    private Outline outline; // Referensi ke komponen Outline

    private void Start()
    {
        // Mendapatkan referensi Outline
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // Nonaktifkan outline secara default
        }
    }

    private void OnMouseEnter()
    {
        // Aktifkan outline saat hover jika komponen Outline ada
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        // Nonaktifkan outline saat kursor keluar dari objek
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

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
