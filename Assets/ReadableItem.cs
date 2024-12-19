using UnityEngine;

public class ReadableItem : MonoBehaviour
{
    public GameObject textPrefab; // Prefab yang sudah ada Text di dalamnya
    public Canvas canvas; // Canvas tempat prefab akan diinstansiasi
    private GameObject instantiatedText; // Instance dari prefab yang diinstansiasi

    void Start()
    {
        if (textPrefab != null && canvas != null)
        {
            instantiatedText = Instantiate(textPrefab, canvas.transform);
            instantiatedText.SetActive(false); // Sembunyikan prefab saat awal
        }
    }

    void OnMouseDown()
    {
        if (instantiatedText != null)
        {
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

