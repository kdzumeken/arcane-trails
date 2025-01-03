using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public Transform spawnPoint;
    public float detectionRadius = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;

    public int attackDamage = 10;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    private Animator animator;
    private Rigidbody rb;

    private bool isFrozen = false;
    private float freezeTimer = 0f;

    private bool isReturning = false;
    private float returnTimer = 3f; // Timer untuk kembali ke posisi spawn

    public HealthBarUI healthBar; // Health bar musuh
    public Color freezeColor = Color.blue;
    public Color normalColor = Color.red;

    public int maxHealth = 50; // Maksimum darah musuh
    private int currentHealth; // Darah saat ini

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth; // Inisialisasi darah musuh

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth); // Atur health bar penuh
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
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            isReturning = false;
            returnTimer = 3f; // Reset timer saat mengejar

            if (distanceToPlayer > attackRange)
            {
                animator.SetBool("ngejar", true);
                animator.SetBool("attack", false);
                ChasePlayer();
            }
            else
            {
                animator.SetBool("ngejar", false);
                animator.SetBool("attack", true);

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    AttackPlayer();
                }
            }
        }
        else if (isReturning)
        {
            returnTimer -= Time.deltaTime;
            if (returnTimer <= 0)
            {
                ReturnToSpawn();
            }
        }
        else
        {
            Idle();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void AttackPlayer()
    {
        if (isFrozen) return; // Abaikan jika musuh sedang freeze

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        animator.SetTrigger("attack"); // Trigger animasi serangan

        // Terapkan damage setelah animasi selesai
        Invoke(nameof(ApplyDamageToPlayer), 0.5f); // Sesuaikan 0.5f dengan durasi animasi serangan
    }

    void ApplyDamageToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            WizardHealth playerHealth = player.GetComponent<WizardHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Player took damage: " + attackDamage);
            }
        }
    }

    void Idle()
    {
        animator.SetBool("ngejar", false);
        animator.SetBool("attack", false);
    }

    void ReturnToSpawn()
    {
        Vector3 direction = (spawnPoint.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Kurangi darah musuh

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); // Perbarui health bar
        }

        // Mainkan animasi damage
        if (animator != null)
        {
            animator.SetTrigger("damage");
        }

        // Jika darah habis, musuh mati
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        animator.SetBool("dead", true); // Jika ada animasi mati (opsional)
        Destroy(gameObject, 2f); // Hapus musuh dari scene setelah animasi selesai
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
                healthBar.fillImage.color = freezeColor;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
