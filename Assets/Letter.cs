using UnityEngine;

public class Letter : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false); // Sembunyikan prefab saat awal
    }

    void OnEnable()
    {
        // Pastikan prefab berada di depan layar saat diaktifkan
        transform.SetAsLastSibling();
    }
}

