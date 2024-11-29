using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    Animator anim;
    float kecepatan = 3f;
    float rotasiKecepatan = 5f;
    float rotasiBatasVertikal = 70f;
    float rotasiVertikal = 0f;
    float rotasiHorizontal = 0f;

    private Vector3 moveAmount;
    private Vector3 smoothMoveVelocity;
    private float smoothRotationVelocity;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 30, 0));
    }

    void Update()
    {
        // Mouse rotation handling
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Update horizontal rotation (without limit)
        rotasiHorizontal += mouseX * rotasiKecepatan;

        // Update vertical rotation (with limit)
        rotasiVertikal -= mouseY * rotasiKecepatan;  // Natural mouse Y behavior
        rotasiVertikal = Mathf.Clamp(rotasiVertikal, -rotasiBatasVertikal, rotasiBatasVertikal);

        // Apply mouse rotation (only vertical is limited, horizontal is free)
        transform.rotation = Quaternion.Euler(rotasiVertikal, rotasiHorizontal, 0);

        // Movement handling
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Calculate the movement direction relative to the camera
        Vector3 moveInput = new Vector3(h, 0f, v).normalized;
        Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;
        Vector3 targetMoveDirection = (forward * moveInput.z + right * moveInput.x).normalized;

        // Check if there is input for movement
        if (h != 0 || v != 0)
        {
            // Smooth movement using Vector3.Lerp
            Vector3 targetVelocity = targetMoveDirection * kecepatan;
            moveAmount = Vector3.Lerp(moveAmount, targetVelocity, Time.deltaTime * 10f); // Increase smoothing factor for smoother transition

            // Apply smooth movement
            transform.position += moveAmount * Time.deltaTime;

            // Smooth rotation towards movement direction
            if (targetMoveDirection != Vector3.zero)
            {
                // Calculate target rotation angle
                float targetRotationY = Mathf.Atan2(targetMoveDirection.x, targetMoveDirection.z) * Mathf.Rad2Deg;

                // Smooth rotation using Quaternion.RotateTowards (more stable than Slerp or SmoothDampAngle)
                Quaternion targetRotation = Quaternion.Euler(0f, targetRotationY, 0f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotasiKecepatan * Time.deltaTime);
            }

            anim.SetBool("jalan", true); // Trigger walking animation
        }
        else
        {
            // Smoothly stop movement
            moveAmount = Vector3.Lerp(moveAmount, Vector3.zero, Time.deltaTime * 10f);
            transform.position += moveAmount * Time.deltaTime;
            anim.SetBool("jalan", false); // Stop walking animation
        }
    }
}
