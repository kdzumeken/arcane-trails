using UnityEngine;
using UnityEngine.AI;

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
    private NavMeshAgent navAgent; // Menggunakan NavMeshAgent untuk navigasi
    private bool isFrozen = false;
    private float freezeTimer = 0f;

    private bool isReturning = false;
    private float returnTimer = 3f; // Timer untuk kembali ke posisi spawn

    public HealthBarMusuh healthBar; // Health bar musuh
    public Color freezeColor = Color.blue;
    public Color normalColor = Color.red;

    public int maxHealth = 50; // Maksimum darah musuh
    private int currentHealth; // Darah saat ini

    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.speed = moveSpeed; // Kecepatan musuh
            navAgent.angularSpeed = rotationSpeed * 100; // Kecepatan rotasi musuh
        }

        currentHealth = maxHealth; // Inisialisasi darah musuh
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
        if (isFrozen || navAgent == null) return;

        navAgent.isStopped = false;
        navAgent.SetDestination(player.position); // Menentukan tujuan ke posisi player
    }

    void AttackPlayer()
    {
        if (isFrozen || navAgent == null) return;

        navAgent.isStopped = true; // Hentikan pergerakan saat menyerang
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
        if (navAgent != null)
        {
            navAgent.isStopped = true; // Hentikan pergerakan saat idle
        }

        animator.SetBool("ngejar", false);
        animator.SetBool("attack", false);
    }

    void ReturnToSpawn()
    {
        if (isFrozen || navAgent == null) return;

        navAgent.isStopped = false;
        navAgent.SetDestination(spawnPoint.position); // Menentukan tujuan ke posisi spawn point
    }

    public void TakeDamage(int damage)
    {
        if (animator != null)
        {
            animator.SetTrigger("damage");
        }

        Freeze(20f);
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
            animator.SetBool("idle", true);

            if (healthBar != null)
            {
                healthBar.fillImage.color = freezeColor;
            }

            if (navAgent != null)
            {
                navAgent.isStopped = true; // Hentikan pergerakan saat freeze
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

        animator.SetBool("idle", false);
        if (navAgent != null)
        {
            navAgent.isStopped = false;
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
