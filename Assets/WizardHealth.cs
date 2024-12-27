using UnityEngine;

public class WizardHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maksimum darah karakter utama
    private int currentHealth; // Darah saat ini

    public HealthBarUI healthBar; // Referensi ke health bar
    public Transform respawnPoint; // Titik respawn karakter

    void Start()
    {
        if (healthBar == null)
        {
            Debug.LogError("HealthBarUI is not assigned in WizardHealth!");
            return;
        }

        currentHealth = maxHealth;           // Inisialisasi darah penuh
        healthBar.SetMaxHealth(maxHealth);   // Set health bar penuh
    }

    public void TakeDamage(int damage)
    {
        if (healthBar == null)
        {
            Debug.LogError("HealthBarUI is not assigned in WizardHealth!");
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
        Debug.Log("Player is dead!");
        Respawn(); // Panggil fungsi respawn
    }

    void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position; // Pindahkan ke titik respawn
            transform.rotation = respawnPoint.rotation; // Set rotasi ke titik respawn
            currentHealth = maxHealth;                 // Pulihkan darah
            healthBar.SetMaxHealth(maxHealth);         // Reset health bar
            Debug.Log("Player respawned at: " + respawnPoint.position);
        }
        else
        {
            Debug.LogError("Respawn point is not assigned!");
        }
    }
}
