using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string itemName; // Nama item
    public Material outlineMaterial; // Material with outline shader
    private Material originalMaterial; // Original material of the object
    private Renderer objectRenderer; // Renderer of the object
    private Material[] originalMaterials; // Original materials for multi-material objects

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            // Jika objek menggunakan beberapa material, kita simpan semua material-nya
            if (objectRenderer.materials.Length > 1)
            {
                originalMaterials = objectRenderer.materials;
            }
            else
            {
                originalMaterial = objectRenderer.material;
            }
        }
    }

    private void OnMouseEnter()
    {
        if (objectRenderer != null && outlineMaterial != null)
        {
            // Ganti material objek dengan outline material saat hover
            if (objectRenderer.materials.Length > 1)
            {
                // Jika objek memiliki beberapa material, ganti hanya material pertama
                Material[] materials = objectRenderer.materials;
                materials[0] = outlineMaterial;
                objectRenderer.materials = materials;
            }
            else
            {
                objectRenderer.material = outlineMaterial;
            }
        }
    }

    private void OnMouseExit()
    {
        if (objectRenderer != null)
        {
            // Kembalikan material asli saat mouse keluar
            if (objectRenderer.materials.Length > 1)
            {
                objectRenderer.materials = originalMaterials;
            }
            else
            {
                objectRenderer.material = originalMaterial;
            }
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
