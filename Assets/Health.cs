using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f; // Nyawa maksimal
    private float currentHealth; // Nyawa saat ini

    public Transform respawnPoint; // Lokasi respawn karakter

    private void Start()
    {
        // Inisialisasi nyawa penuh saat mulai
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        // Kurangi nyawa
        currentHealth -= amount;
        Debug.Log("Current Health: " + currentHealth);

        // Cek jika nyawa habis
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Character Died!");

        // Pastikan karakter dipindahkan ke posisi aman
        Vector3 respawnPosition = respawnPoint.position;

        // Cek apakah posisi respawn aman
        if (Physics.CheckSphere(respawnPosition, 0.5f, LayerMask.GetMask("Ground")))
        {
            // Posisi aman, pindahkan ke respawnPoint
            transform.position = respawnPosition;
            Debug.Log("Respawned at Safe Position!");
        }
        else
        {
            Debug.LogWarning("Respawn position is not safe. Adjust RespawnPoint position.");
        }

        // Reset nyawa
        currentHealth = maxHealth;

        Debug.Log("Respawned with Full Health!");
    }

}
