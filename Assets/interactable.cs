using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    public string itemName; // Nama item
    public GameObject itemObject; // Objek 3D item
    public Sprite itemSprite; // Sprite item
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
        // Mendapatkan referensi ke inventory
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            // Menambahkan item ke inventory dan mengupdate UI
            inventory.AddItem(itemName, itemObject, itemSprite, displayName); // Tambahkan displayName
            Destroy(gameObject); // Hapus objek setelah diambil
            Debug.Log($"Item '{itemName}' dengan nama tampilan '{displayName}' ditambahkan ke inventory.");
        }

        // Sembunyikan UI teks setelah item diambil
        if (hoverText != null)
        {
            hoverText.gameObject.SetActive(false);
        }
    }
}