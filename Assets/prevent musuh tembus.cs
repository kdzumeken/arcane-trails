using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preventmusuhtembus : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint; // Reference to the spawn point

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Move the enemy back to the spawn point
            other.transform.position = spawnPoint.position;
            other.transform.rotation = spawnPoint.rotation;

            // Optionally, reset the enemy's velocity if it has a Rigidbody
            Rigidbody enemyRigidbody = other.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.velocity = Vector3.zero;
                enemyRigidbody.angularVelocity = Vector3.zero;
            }
        }
    }
}
