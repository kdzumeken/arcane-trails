using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{
    public class PuzzleApple : MonoBehaviour
    {
        public LeftAltar leftAltar; // Reference to the left altar script
        public RightAltar rightAltar; // Reference to the right altar script
        public GameObject key; // The key that will fall when the puzzle is solved
        public float keyDropHeight = 5.0f; // Height from which the key will drop

        void Start()
        {
            if (key != null)
            {
                key.SetActive(false); // Hide the key initially
            }
        }

        void Update()
        {
            if (leftAltar.IsItemPlaced() && rightAltar.IsItemPlaced())
            {
                DropKey();
            }
        }

        void DropKey()
        {
            if (key != null)
            {
                key.SetActive(true);
                key.transform.position = new Vector3(
                    (leftAltar.transform.position.x + rightAltar.transform.position.x) / 2,
                    leftAltar.transform.position.y + keyDropHeight,
                    (leftAltar.transform.position.z + rightAltar.transform.position.z) / 2
                );

                // Remove Rigidbody if it exists to prevent the key from falling
                Rigidbody rb = key.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Destroy(rb);
                }

                Debug.Log("The key has appeared.");
            }
        }
    }
}
