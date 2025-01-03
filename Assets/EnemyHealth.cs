using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50; // Maksimum darah musuh
    private int currentHealth; // Darah saat ini

    public HealthBarUI healthBar; // Referensi ke health bar

    void Start()
    {
        if (healthBar == null)
        {
            Debug.LogError("HealthBarUI is not assigned in EnemyHealth!");
            return;
        }

        currentHealth = maxHealth;           // Inisialisasi darah penuh
        healthBar.SetMaxHealth(maxHealth);   // Set health bar penuh
    }

    public void TakeDamage(int damage)
    {
        if (healthBar == null)
        {
            Debug.LogError("HealthBarUI is not assigned in EnemyHealth!");
            return;
        }

        currentHealth -= damage;            // Kurangi darah
        currentHealth = Mathf.Max(currentHealth, 0); // Cegah nilai negatif
        healthBar.SetHealth(currentHealth); // Update health bar

        if (currentHealth <= 0)
        {
            Die(); // Panggil fungsi mati
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " is dead!");
        Destroy(gameObject); // Hapus musuh dari scene
    }
}