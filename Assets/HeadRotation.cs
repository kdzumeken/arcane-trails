using UnityEngine;

public class HeadRotation : MonoBehaviour
{
    public Transform head; // Referensi ke GameObject kepala
    public float rotationSpeed = 5f; // Kecepatan rotasi
    public float minAngle = -30f; // Batas rotasi ke bawah
    public float maxAngle = 30f;  // Batas rotasi ke atas

    void Update()
    {
        // Ambil input vertikal untuk rotasi atas/bawah
        float verticalInput = Input.GetAxis("Mouse Y");

        // Rotasi kepala berdasarkan input
        float newRotation = head.localRotation.eulerAngles.x - verticalInput * rotationSpeed;

        // Batasi rotasi kepala agar tidak berlebihan
        newRotation = Mathf.Clamp(newRotation, minAngle, maxAngle);

        // Terapkan rotasi kepala
        head.localRotation = Quaternion.Euler(newRotation, 0, 0);
    }
}
