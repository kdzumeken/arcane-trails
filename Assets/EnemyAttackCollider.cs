using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    public EnemyBehavior enemyBehavior; // Referensi ke script musuh

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Jika mengenai karakter utama
        {
            WizardHealth wizardHealth = other.GetComponent<WizardHealth>();
            if (wizardHealth != null)
            {
                wizardHealth.TakeDamage(enemyBehavior.attackDamage); // Berikan damage
            }
        }
    }
}