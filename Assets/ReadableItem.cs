using UnityEngine;

public class ReadableItem : MonoBehaviour
{
    public GameObject textPrefab; // Prefab yang sudah ada Text di dalamnya
    public Canvas canvas; // Canvas tempat prefab akan diinstansiasi
    private GameObject instantiatedText; // Instance dari prefab yang diinstansiasi
    private Outline outline; // Referensi ke komponen Outline

    void Start()
    {
        // Mendapatkan referensi Outline
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // Nonaktifkan outline secara default
        }

        if (textPrefab != null && canvas != null)
        {
            instantiatedText = Instantiate(textPrefab, canvas.transform);
            instantiatedText.SetActive(false); // Sembunyikan prefab saat awal
        }
    }

    void OnMouseEnter()
    {
        // Aktifkan outline saat hover jika komponen Outline ada
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    void OnMouseExit()
    {
        // Nonaktifkan outline saat kursor keluar dari objek
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    void OnMouseDown()
    {
        if (instantiatedText != null)
        {
            // Nonaktifkan semua instance teks lainnya
            ReadableItem[] readableItems = FindObjectsOfType<ReadableItem>();
            foreach (ReadableItem item in readableItems)
            {
                if (item.instantiatedText != null)
                {
                    item.instantiatedText.SetActive(false);
                }
            }

            instantiatedText.SetActive(true); // Tampilkan prefab
        }
    }

    void Update()
    {
        if (instantiatedText != null && instantiatedText.activeSelf && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))) // Klik kanan atau tekan Esc untuk menutup
        {
            instantiatedText.SetActive(false); // Sembunyikan prefab
        }
    }
}
