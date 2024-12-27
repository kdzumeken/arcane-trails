using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player; // Referensi ke karakter utama
    public float detectionRadius = 10f; // Radius radar deteksi musuh
    public float attackRange = 2f; // Jarak untuk menyerang
    public float moveSpeed = 3f; // Kecepatan musuh saat mengejar
    public float rotationSpeed = 5f; // Kecepatan rotasi musuh
    private bool isAttacking = false; // Apakah musuh sedang menyerang

    public int attackDamage = 10; // Damage yang diberikan musuh
    public float attackCooldown = 1.5f; // Waktu cooldown antar serangan
    private float lastAttackTime; // Waktu terakhir musuh menyerang

    private Animator animator; // Animator musuh
    private Rigidbody rb; // Rigidbody musuh

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true; // Mengatur Rigidbody menjadi kinematic jika ada
        }
        else
        {
            Debug.LogWarning("Rigidbody not found on " + gameObject.name);
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            if (distanceToPlayer > attackRange)
            {
                isAttacking = false;
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
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
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
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Pastikan Collider atau Trigger menyerang karakter utama
        Debug.Log("Enemy is attacking!");
    }

    void Idle()
    {
        animator.SetBool("ngejar", false);
        animator.SetBool("attack", false);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}