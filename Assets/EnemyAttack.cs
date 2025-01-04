using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 10; // Damage yang diberikan musuh
    public float attackCooldown = 1.5f; // Waktu jeda antar serangan
    private float nextAttackTime = 0f; // Waktu berikutnya untuk menyerang

    private EnemyHealth enemyHealth;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component is missing in EnemyAttack!");
            // Coba cari komponen di parent atau child
            enemyHealth = GetComponentInParent<EnemyHealth>();
            if (enemyHealth == null)
            {
                enemyHealth = GetComponentInChildren<EnemyHealth>();
                if (enemyHealth == null)
                {
                    Debug.LogError("EnemyHealth component is still missing after searching in parent and children!");
                }
                else
                {
                    Debug.Log("EnemyHealth component found in children.");
                }
            }
            else
            {
                Debug.Log("EnemyHealth component found in parent.");
            }
        }
        else
        {
            Debug.Log("EnemyHealth component found in the same object.");
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Deteksi apakah musuh berada di dekat karakter utama
        if (other.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            // Cek apakah musuh dalam status freeze
            if (enemyHealth != null && enemyHealth.IsFrozen())
            {
                return; // Jangan menyerang jika musuh dalam status freeze
            }

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