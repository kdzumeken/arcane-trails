using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    Animator anim;
    float kecepatan = 3f;
    float rotasiKecepatan = 2f;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 30, 0));
        Debug.Log("Rotasi Hero: " + this.transform.eulerAngles.y);
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            // Tentukan arah target
            Vector3 targetDirection = new Vector3(h, 0f, v);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;

            // Rotasi secara bertahap menggunakan Slerp
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * rotasiKecepatan);

            // Pergerakan karakter dengan arah input
            Vector3 moveDirection = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up) * v +
                                    Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up) * h;
            this.transform.position += moveDirection.normalized * Time.deltaTime * kecepatan;

            anim.SetBool("jalan", true);
        }
        else
        {
            anim.SetBool("jalan", false);
        }
    }
}
