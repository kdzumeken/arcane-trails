using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50; // Maksimum darah musuh
    private int currentHealth; // Darah saat ini

    public HealthBarMusuh healthBar; // Referensi ke health bar
    private Animator animator; // Referensi ke komponen Animator
    private bool isFrozen = false; // Status freeze
    private float freezeTimer = 0f; // Timer untuk freeze

    void Start()
    {
        if (healthBar == null)
        {
            Debug.LogError("HealthBarUI is not assigned in EnemyHealth!");
            return;
        }

        currentHealth = maxHealth;           // Inisialisasi darah penuh
        healthBar.SetMaxHealth(maxHealth);   // Set health bar penuh

        animator = GetComponent<Animator>(); // Inisialisasi komponen Animator
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned in EnemyHealth!");
        }
    }

    void Update()
    {
        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0)
            {
                Unfreeze();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (healthBar == null)
        {
            Debug.LogError("HealthBarUI is not assigned in EnemyHealth!");
            return;
        }

        // Mainkan animasi damage
        if (animator != null)
        {
            animator.SetTrigger("damage");
        }

        // Bekukan musuh selama 20 detik
        Freeze(20f);
    }

    public void Freeze(float duration)
    {
        if (!isFrozen)
        {
            isFrozen = true;
            freezeTimer = duration;

            animator.SetBool("ngejar", false);
            animator.SetBool("attack", false);

            if (healthBar != null)
            {
                healthBar.fillImage.color = Color.blue; // Ubah warna health bar saat freeze
            }
        }
    }

    private void Unfreeze()
    {
        isFrozen = false;

        if (healthBar != null)
        {
            healthBar.UpdateColor(); // Perbarui warna health bar sesuai kondisi darah
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " is dead!");
        Destroy(gameObject); // Hapus musuh dari scene
    }
}
