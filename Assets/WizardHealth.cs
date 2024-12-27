using UnityEngine;

public class WizardHealth : MonoBehaviour
{
    public int maxHealth = 100; // HP maksimum karakter utama
    private int currentHealth; // HP saat ini
    public Transform respawnPoint; // Titik respawn jika karakter mati

    void Start()
    {
        currentHealth = maxHealth; // Inisialisasi HP penuh
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Kurangi HP
        Debug.Log("Wizard HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Wizard is dead! Respawning...");
        transform.position = respawnPoint.position; // Respawn ke titik awal
        currentHealth = maxHealth; // Reset HP
    }
}