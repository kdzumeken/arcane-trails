using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{
    public class BallManager : MonoBehaviour
    {
        public static BallManager Instance;

        private int collectedBalls = 0;
        public int requiredBalls = 3;
        public GameObject keyPrefab;
        public Transform keySpawnPosition;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void CollectBall()
        {
            collectedBalls++;
            Debug.Log("Collected Ball: " + collectedBalls + "/" + requiredBalls);

            if (collectedBalls >= requiredBalls)
            {
                SpawnKey();
            }
        }

        private void SpawnKey()
        {
            if (keyPrefab != null && keySpawnPosition != null)
            {
                Instantiate(keyPrefab, keySpawnPosition.position, Quaternion.identity);
                Debug.Log("Key has appeared.");
            }
        }
    }
}
