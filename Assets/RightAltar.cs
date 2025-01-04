using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{
    public class RightAltar : MonoBehaviour
    {
        public string requiredItem = "RedApple"; // Item required for the right altar
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
                Debug.Log("Player entered right altar trigger area.");
                playerInRange = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player exited right altar trigger area.");
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
                    // Place the item in the scene
                    GameObject itemObject = new GameObject(requiredItem);
                    itemObject.transform.position = itemPosition.position;
                    Debug.Log(requiredItem + " has been placed on the right altar.");

                    // After item is placed, remove it and its associated elements
                    RemoveItemAndAllRelated(inventory, requiredItem);

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

        // Method to remove the item and its related elements
        void RemoveItemAndAllRelated(Inventory inventory, string itemName)
        {
            // Find the index of the item to remove
            int index = inventory.items.IndexOf(itemName);

            // Ensure the item exists in the list
            if (index >= 0)
            {
                // First, remove the associated item data from other lists
                if (index < inventory.itemObjects.Count)
                {
                    // Deactivate or destroy the associated GameObject if needed
                    GameObject itemObject = inventory.itemObjects[index];
                    if (itemObject != null)
                    {
                        itemObject.SetActive(false); // Optionally destroy it: Destroy(itemObject);
                    }
                }

                // Now, remove the item and its related elements from the lists
                inventory.itemObjects.RemoveAt(index);
                inventory.itemSprites.RemoveAt(index);
                inventory.displayNames.RemoveAt(index);

                // Finally, remove the item from the inventory
                inventory.items.RemoveAt(index);

                // Debugging message
                Debug.Log(itemName + " has been completely removed from the inventory.");
            }
            else
            {
                // If the item wasn't found
                Debug.LogWarning(itemName + " not found in inventory.");
            }
        }

    }
}