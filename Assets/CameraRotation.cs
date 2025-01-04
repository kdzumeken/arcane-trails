using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Kecepatan rotasi kamera
    public float minAngle = -30f;     // Batas rotasi ke bawah
    public float maxAngle = 30f;      // Batas rotasi ke atas

    private float currentRotationX = 0f;  // Menyimpan rotasi vertikal saat ini
    private float initialRotationY;       // Menyimpan rotasi horizontal awal
    private float initialRotationZ;       // Menyimpan rotasi Z awal

    void Start()
    {
        // Simpan rotasi awal Y dan Z
        initialRotationY = transform.localRotation.eulerAngles.y;
        initialRotationZ = transform.localRotation.eulerAngles.z;
    }

    void Update()
    {
        // Ambil input vertikal (pergerakan mouse di Y-axis)
        float mouseY = Input.GetAxis("Mouse Y");

        // Hitung rotasi baru untuk vertikal kamera
        currentRotationX -= mouseY * rotationSpeed;

        // Batasi rotasi agar tidak melebihi batas yang ditentukan
        currentRotationX = Mathf.Clamp(currentRotationX, minAngle, maxAngle);

        // Terapkan rotasi pada kamera di sumbu X, pastikan rotasi Y dan Z tetap pada nilai awal
        transform.localRotation = Quaternion.Euler(currentRotationX, initialRotationY, initialRotationZ);
    }
}
