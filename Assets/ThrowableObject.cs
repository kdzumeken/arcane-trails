using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    public int damage = 10; // Damage yang diberikan oleh objek throwable
    public float freezeDuration = 5f; // Durasi freeze musuh

    void OnCollisionEnter(Collision collision)
    {
        // Cek apakah objek yang terkena memiliki tag "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Ambil komponen EnemyBehavior dari musuh
            EnemyBehavior enemy = collision.gameObject.GetComponent<EnemyBehavior>();

            if (enemy != null) // Pastikan EnemyBehavior ditemukan
            {
                enemy.TakeDamage(damage); // Kurangi darah musuh
                enemy.Freeze(freezeDuration); // Masukkan musuh ke status freeze
            }
        }

        // Hancurkan objek setelah mengenai sesuatu
        Destroy(gameObject);
    }
}
