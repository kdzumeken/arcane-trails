using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotation : MonoBehaviour
{
    public float rotasiKecepatan = 5f; // Kecepatan rotasi kepala
    public float rotasiBatasVertikal = 70f; // Batas maksimal rotasi vertikal kepala
    private float rotasiVertikal = 0f; // Variabel untuk rotasi vertikal kepala

    void Update()
    {
        // Ambil input mouse Y untuk rotasi vertikal (atas-bawah)
        float mouseY = Input.GetAxis("Mouse Y");

        // Update rotasi vertikal kepala (atas-bawah)
        rotasiVertikal -= mouseY * rotasiKecepatan;

        // Batasi rotasi kepala hanya sampai rotasiBatasVertikal
        rotasiVertikal = Mathf.Clamp(rotasiVertikal, -rotasiBatasVertikal, rotasiBatasVertikal);

        // Set rotasi kepala menggunakan rotasi vertikal (atas-bawah)
        transform.localRotation = Quaternion.Euler(rotasiVertikal, 0f, 0f);
    }
}
