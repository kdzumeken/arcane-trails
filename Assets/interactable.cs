using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    public string itemName; // Nama item
    public string displayName; // Nama yang ditampilkan di UI
    public TextMeshProUGUI hoverText; // Referensi ke TMP Text untuk menampilkan nama item
    private Outline outline; // Referensi ke komponen Outline

    private void Start()
    {
        // Mendapatkan referensi Outline
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // Nonaktifkan outline secara default
        }

        // Nonaktifkan hoverText secara default
        if (hoverText != null)
        {
            hoverText.gameObject.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        // Aktifkan outline saat hover jika komponen Outline ada
        if (outline != null)
        {
            outline.enabled = true;
        }

        // Tampilkan UI teks saat hover
        if (hoverText != null)
        {
            hoverText.text = "Take " + displayName;
            hoverText.gameObject.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        // Nonaktifkan outline saat kursor keluar dari objek
        if (outline != null)
        {
            outline.enabled = false;
        }

        // Sembunyikan UI teks saat kursor keluar dari objek
        if (hoverText != null)
        {
            hoverText.gameObject.SetActive(false);
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

        // Sembunyikan UI teks setelah item diambil
        if (hoverText != null)
        {
            hoverText.gameObject.SetActive(false);
        }
    }
}

