using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    public float damagePerSecond = 20f; // Damage per detik
    public string targetTag = "Player"; // Tag untuk karakter target

    private void OnTriggerStay(Collider other)
    {
        // Periksa apakah objek yang berada di trigger memiliki tag sesuai
        if (other.CompareTag(targetTag))
        {
            // Ambil komponen Health dari objek
            Health characterHealth = other.GetComponent<Health>();
            if (characterHealth != null)
            {
                // Berikan damage secara perlahan
                characterHealth.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
