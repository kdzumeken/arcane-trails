using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{
    public class LeftAltar : MonoBehaviour
    {
        public string requiredItem = "RedApple"; // Item required for the left altar
        public Transform itemPosition; // Position where the item will be placed
        private bool itemPlaced = false;
        private bool playerInRange = false;

        public bool IsItemPlaced()
        {
            return itemPlaced;
        }

        void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E) && !itemPlaced)
            {
                TryToPlaceItem();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player entered left altar trigger area.");
                playerInRange = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player exited left altar trigger area.");
                playerInRange = false;
            }
        }

        void TryToPlaceItem()
        {
            Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            if (inventory != null)
            {
                if (inventory.items.Contains(requiredItem))
                {
                    // Remove the item from the inventory
                    inventory.items.Remove(requiredItem);
                    Debug.Log(requiredItem + " has been used and removed from the inventory.");

                    // Place the item in the scene
                    GameObject itemObject = new GameObject(requiredItem);
                    itemObject.transform.position = itemPosition.position;
                    Debug.Log(requiredItem + " has been placed on the left altar.");

                    itemPlaced = true;
                }
                else
                {
                    Debug.Log("Required item " + requiredItem + " not found in inventory.");
                }
            }
            else
            {
                Debug.Log("Inventory component not found on player.");
            }
        }
    }
}
