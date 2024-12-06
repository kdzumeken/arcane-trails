using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    Animator anim;
    public float kecepatan = 3f;
    public float kecepatanLari = 6f; // Kecepatan saat berlari
    public float rotasiKecepatan = 5f;
    public float kekuatanLompat = 5f; // Kekuatan lompatan
    private Vector3 moveAmount;

    // Referensi untuk kepala
    public Transform headTransform;
    private float rotasiHorizontal = 0f;

    private Rigidbody rb; // Rigidbody untuk lompat

    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Input pergerakan karakter
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Hitung arah pergerakan relatif terhadap kamera
        Vector3 moveInput = new Vector3(h, 0f, v).normalized;
        Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;
        Vector3 targetMoveDirection = (forward * moveInput.z + right * moveInput.x).normalized;

        // Kecepatan sesuai apakah sedang lari atau tidak
        float kecepatanAktual = Input.GetKey(KeyCode.LeftShift) ? kecepatanLari : kecepatan;

        // Cek jika ada input untuk pergerakan
        if (h != 0 || v != 0)
        {
            // Smooth movement menggunakan Vector3.Lerp
            Vector3 targetVelocity = targetMoveDirection * kecepatanAktual;
            moveAmount = Vector3.Lerp(moveAmount, targetVelocity, Time.deltaTime * 10f);
            transform.position += moveAmount * Time.deltaTime;

            anim.SetBool("jalan", true); // Trigger animasi jalan
            anim.SetBool("lari", Input.GetKey(KeyCode.LeftShift)); // Trigger animasi lari jika Shift ditekan
        }
        else
        {
            // Smoothly stop movement
            moveAmount = Vector3.Lerp(moveAmount, Vector3.zero, Time.deltaTime * 10f);
            transform.position += moveAmount * Time.deltaTime;
            anim.SetBool("jalan", false); // Stop walking animation
            anim.SetBool("lari", false); // Stop running animation
        }

        // Rotasi tubuh berdasarkan input mouse horizontal (sumbu Y)
        float mouseX = Input.GetAxis("Mouse X");
        rotasiHorizontal += mouseX * rotasiKecepatan;
        transform.rotation = Quaternion.Euler(0f, rotasiHorizontal, 0f);

        // Lompatan jika Space ditekan dan karakter di tanah
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * kekuatanLompat, ForceMode.Impulse);
            anim.SetTrigger("lompat"); // Trigger animasi lompat
        }
    }

    // Cek apakah karakter berada di tanah
    bool IsGrounded()
    {
        // Raycast ke bawah untuk memeriksa jika karakter di tanah
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
