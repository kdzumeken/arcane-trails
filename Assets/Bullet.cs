using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 10f;
    public string enemyTag = "Enemy";
    public float damage = 10f;

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Bullet hit: {other.gameObject.name}");
        if (other.gameObject.CompareTag(enemyTag))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log($"Damage applied to {other.gameObject.name}");
                enemyHealth.TakeDamage((int)damage); // Convert float to int
                enemyHealth.Freeze(20f); // Memicu efek freeze selama 20 detik
            }
            else
            {
                Debug.LogWarning("No EnemyHealth script attached to target!");
            }
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"Hit object with tag: {other.tag}");
        }
    }
}