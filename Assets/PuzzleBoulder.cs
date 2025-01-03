using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{
    public class PuzzleBoulder : MonoBehaviour
    {
        public Altar altar; // Reference to the altar script
        public GameObject key; // The key that will appear when the puzzle is solved
        public Transform keyPosition; // Position where the key should appear

        void Start()
        {
            if (key != null)
            {
                key.SetActive(false); // Hide the key initially
            }
        }

        void Update()
        {
            if (altar.IsItemPlaced())
            {
                DropKey();
            }
        }

        void DropKey()
        {
            if (key != null && keyPosition != null)
            {
                key.SetActive(true);
                key.transform.position = keyPosition.position; // Set key position to keyPosition

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

