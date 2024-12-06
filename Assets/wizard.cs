using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    Animator anim;
    public float kecepatan = 3f;
    public float rotasiKecepatan = 5f;
    public float kecepatanLari = 6f; // Kecepatan saat berlari
    public float jumpForce = 5f; // Kekuatan lompatan
    private Vector3 moveAmount;
    private Rigidbody rb;

    // Referensi untuk kepala
    public Transform headTransform;
    private float rotasiHorizontal = 0f;

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

        // Cek jika ada input untuk pergerakan
        if (h != 0 || v != 0)
        {
            // Cek jika tombol Shift ditekan untuk berlari
            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float currentSpeed = isRunning ? kecepatanLari : kecepatan;

            // Smooth movement menggunakan Vector3.Lerp
            Vector3 targetVelocity = targetMoveDirection * currentSpeed;
            moveAmount = Vector3.Lerp(moveAmount, targetVelocity, Time.deltaTime * 10f);
            transform.position += moveAmount * Time.deltaTime;

            anim.SetBool("jalan", !isRunning); // Trigger animasi jalan jika tidak berlari
            anim.SetBool("lari", isRunning); // Trigger animasi lari jika berlari
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

        // Cek jika tombol spasi ditekan untuk lompat
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Jump()
    {
        // Cek jika karakter berada di tanah sebelum melompat
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("lompat"); // Trigger animasi lompat
            StartCoroutine(ResetJumpTrigger());
        }
    }

    IEnumerator ResetJumpTrigger()
    {
        // Tunggu hingga animasi lompat selesai
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Reset trigger lompat
        anim.ResetTrigger("lompat");

        // Debug log to check if the coroutine is running
        Debug.Log("ResetJumpTrigger coroutine running");

        // Kembali ke animasi yang sesuai
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            anim.SetBool("jalan", !isRunning);
            anim.SetBool("lari", isRunning);
        }
        else
        {
            anim.SetBool("jalan", false);
            anim.SetBool("lari", false);
        }
    }

    bool IsGrounded()
    {
        // Cek jika karakter berada di tanah menggunakan raycast
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
