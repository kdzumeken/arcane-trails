using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float mouseSensitivity = 100f; // Sensitivitas mouse
    public Transform playerBody;         // Referensi ke badan karakter utama

    private float xRotation = 0f;         // Rotasi kamera di sumbu X
    public float maxLookAngle = 80f;     // Batas maksimum pandangan ke atas dan ke bawah

    void Start()
    {
        // Kunci kursor di tengah layar
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Ambil input mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotasi kamera di sumbu X (atas/bawah)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle); // Batasi rotasi

        // Terapkan rotasi hanya pada rotasi sumbu X tanpa memengaruhi posisi kamera
        transform.localEulerAngles = new Vector3(xRotation, transform.localEulerAngles.y, transform.localEulerAngles.z);

        // Rotasi badan karakter di sumbu Y (kiri/kanan)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
