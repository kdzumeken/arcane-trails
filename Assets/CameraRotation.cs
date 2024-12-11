using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float rotasiKecepatan = 5f; // Kecepatan rotasi kamera
    public float rotasiBatasVertikal = 70f; // Batas maksimal rotasi vertikal
    private float rotasiVertikal = 0f; // Variabel untuk rotasi vertikal kamera

    private Transform karakterTransform; // Referensi ke transform karakter

    void Start()
    {
        karakterTransform = transform.parent; // Ambil referensi ke transform karakter (parent)
    }

    void Update()
    {
        // Ambil input mouse Y untuk rotasi vertikal (atas-bawah) kamera
        float mouseY = Input.GetAxis("Mouse Y");

        // Update rotasi vertikal kamera (atas-bawah)
        rotasiVertikal -= mouseY * rotasiKecepatan;

        // Batasi rotasi kamera hanya sampai rotasiBatasVertikal
        rotasiVertikal = Mathf.Clamp(rotasiVertikal, -rotasiBatasVertikal, rotasiBatasVertikal);

        // Set rotasi kamera menggunakan rotasi vertikal (atas-bawah)
        transform.localRotation = Quaternion.Euler(rotasiVertikal, 0f, 0f);

        // Rotasi karakter (tubuh) mengikuti rotasi horizontal dari mouse X
        float mouseX = Input.GetAxis("Mouse X");
        karakterTransform.Rotate(Vector3.up * mouseX * rotasiKecepatan);
    }
}
