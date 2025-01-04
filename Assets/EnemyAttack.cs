using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 10; // Damage yang diberikan musuh
    public float attackCooldown = 1.5f; // Waktu jeda antar serangan
    private float nextAttackTime = 0f; // Waktu berikutnya untuk menyerang

    void OnTriggerStay(Collider other)
    {
        // Deteksi apakah musuh berada di dekat karakter utama
        if (other.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            // Ambil komponen WizardHealth dari karakter
            WizardHealth wizardHealth = other.GetComponent<WizardHealth>();
            if (wizardHealth != null)
            {
                wizardHealth.TakeDamage(attackDamage); // Kurangi darah karakter
                Debug.Log("Enemy attacked the player!");
                nextAttackTime = Time.time + attackCooldown; // Atur waktu serangan berikutnya
            }
        }
    }
}
