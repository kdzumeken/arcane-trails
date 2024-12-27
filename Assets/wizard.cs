using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Crosshair
    [SerializeField] private Image crosshair;

    // Referensi untuk objek yang disorot
    private InteractableObject highlightedObject;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Create crosshair if not assigned in Inspector
        if (crosshair == null)
        {
            GameObject crosshairObject = new GameObject("Crosshair");
            crosshairObject.transform.SetParent(GameObject.Find("Canvas").transform);
            crosshair = crosshairObject.AddComponent<Image>();
            crosshair.rectTransform.sizeDelta = new Vector2(20, 20);
            crosshair.color = Color.white;

            // Set crosshair image (you can replace this with your own crosshair sprite)
            crosshair.sprite = Resources.Load<Sprite>("CrosshairSprite");
            crosshair.rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    void HandleMovement()
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
            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float currentSpeed = isRunning ? kecepatanLari : kecepatan;

            Vector3 targetVelocity = targetMoveDirection * currentSpeed;
            moveAmount = Vector3.Lerp(moveAmount, targetVelocity, Time.deltaTime * 10f);
            transform.position += moveAmount * Time.deltaTime;

            anim.SetBool("jalan", !isRunning);
            anim.SetBool("lari", isRunning);
        }
        else
        {
            moveAmount = Vector3.Lerp(moveAmount, Vector3.zero, Time.deltaTime * 10f);
            transform.position += moveAmount * Time.deltaTime;
            anim.SetBool("jalan", false);
            anim.SetBool("lari", false);
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

    void HandleInteraction()
    {
        // Raycast dari kamera untuk mendeteksi objek yang dapat diinteraksi
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f)) // Radius interaksi = 5 unit
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();

            if (interactable != null)
            {
                highlightedObject = interactable;

                // Jika tombol kiri mouse ditekan
                if (Input.GetMouseButtonDown(0)) // 0 = klik kiri
                {
                    Inventory inventory = FindObjectOfType<Inventory>();
                    if (inventory != null)
                    {
                        // Tambahkan item ke inventory
                        inventory.AddItem(interactable.itemName, interactable.itemObject, interactable.itemSprite, interactable.displayName);

                        // Nonaktifkan objek setelah diambil
                        interactable.gameObject.SetActive(false);

                        Debug.Log($"Item '{interactable.displayName}' ditambahkan ke inventory.");
                    }
                }
            }
        }
        else
        {
            highlightedObject = null;
        }
    }


    void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("lompat");
        }
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
    }
}
