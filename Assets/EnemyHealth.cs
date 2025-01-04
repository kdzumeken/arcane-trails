using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    public HealthBarMusuh healthBar;
    private Animator animator;
    private bool isFrozen = false;
    private float freezeTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned in EnemyHealth!");
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
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 1; // Musuh tidak mati, tetap hidup dengan 1 HP
        }

        if (animator != null)
        {
            animator.SetTrigger("damage");
        }

        Freeze(20f);
    }

    public void Freeze(float duration)
    {
        if (!isFrozen)
        {
            isFrozen = true;
            freezeTimer = duration;

            // Hentikan animasi attack dan ngejar, dan atur animasi idle
            animator.SetBool("ngejar", false);
            animator.SetBool("attack", false);
            animator.SetBool("idle", true);

            if (healthBar != null)
            {
                healthBar.fillImage.color = new Color(0.6f, 0.8f, 1f);
            }
        }
    }

    private void Unfreeze()
    {
        isFrozen = false;

        if (healthBar != null)
        {
            healthBar.UpdateColor();
        }

        animator.SetBool("idle", false);
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }
}