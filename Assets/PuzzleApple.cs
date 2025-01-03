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

        private bool keyDropped = false; // To ensure the key drops only once

        void Start()
        {
            if (key != null)
            {
                key.SetActive(false); // Hide the key initially
            }
        }

        void Update()
        {
            // Check if both altars have their items placed
            if (leftAltar.IsItemPlaced() && rightAltar.IsItemPlaced() && !keyDropped)
            {
                DropKey(); // Drop the key if both altars are filled
            }
        }

        void DropKey()
        {
            if (key != null)
            {
                key.SetActive(true); // Show the key
                key.transform.position = new Vector3(
                    (leftAltar.transform.position.x + rightAltar.transform.position.x) / 2,
                    leftAltar.transform.position.y + keyDropHeight,
                    (leftAltar.transform.position.z + rightAltar.transform.position.z) / 2
                );

                Rigidbody rb = key.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Destroy(rb); // Remove Rigidbody if it exists
                }

                keyDropped = true; // Mark the key as dropped
                Debug.Log("The key has appeared.");
            }
        }
    }
}