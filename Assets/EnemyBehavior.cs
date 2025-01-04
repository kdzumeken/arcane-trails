using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player; // Referensi ke karakter utama
    public float detectionRadius = 10f; // Radius radar deteksi musuh
    public float attackRange = 2f; // Jarak untuk menyerang
    public float moveSpeed = 3f; // Kecepatan musuh saat mengejar
    public float rotationSpeed = 5f; // Kecepatan rotasi musuh

    public int attackDamage = 10; // Damage yang diberikan musuh
    public float attackCooldown = 1.5f; // Waktu cooldown antar serangan
    private float lastAttackTime; // Waktu terakhir musuh menyerang

    private Animator animator; // Animator musuh
    private Rigidbody rb; // Rigidbody musuh

    private bool isAttacking = false; // Apakah musuh sedang menyerang
    private bool isChasing = false; // Apakah musuh sedang mengejar

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true; // Mengatur Rigidbody menjadi kinematic
        }
        else
        {
            Debug.LogWarning("Rigidbody not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player transform is not assigned!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            if (distanceToPlayer > attackRange)
            {
                // Mengejar pemain
                isAttacking = false;
                isChasing = true;
                SetAnimatorStates(isChasing: true, isAttacking: false);
                ChasePlayer();
            }
            else
            {
                // Menyerang pemain
                isChasing = false;
                SetAnimatorStates(isChasing: false, isAttacking: true);

                if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    AttackPlayer();
                }
            }
        }
        else
        {
            // Kembali ke idle
            SetAnimatorStates(isChasing: false, isAttacking: false);
            isAttacking = false;
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
        isAttacking = true;

        // Pastikan Rigidbody berhenti untuk menyerang
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Debug untuk menunjukkan musuh menyerang
        Debug.Log("Enemy is attacking!");

        // Terapkan damage ke pemain (memerlukan komponen pemain dengan skrip health)
        ApplyDamageToPlayer();
    }

    void ApplyDamageToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            var playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Player took damage: " + attackDamage);
            }
        }
    }

    void SetAnimatorStates(bool isChasing, bool isAttacking)
    {
        animator.SetBool("ngejar", isChasing);
        animator.SetBool("attack", isAttacking);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
