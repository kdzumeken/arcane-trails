using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10; // Besar damage serangan

    void OnTriggerEnter(Collider other)
    {
        // Cek apakah objek yang terkena adalah musuh
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage); // Kurangi darah musuh
                Debug.Log("Enemy hit!");
            }
        }
    }
}
