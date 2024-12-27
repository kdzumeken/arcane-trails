using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{
    public class Altar : MonoBehaviour
    {
        public string requiredItem = "Statue"; // Item required for the right altar
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
                    // Remove the item and its associated elements from the inventory
                    RemoveItemAndAllRelated(inventory, requiredItem);

                    // Place the item in the scene
                    GameObject itemObject = new GameObject(requiredItem);
                    itemObject.transform.position = itemPosition.position;
                    Debug.Log(requiredItem + " has been placed on the right altar.");

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

        void RemoveItemAndAllRelated(Inventory inventory, string itemName)
        {
            int index = inventory.items.IndexOf(itemName);
            if (index >= 0)
            {
                // Remove the item and its associated elements from all lists
                inventory.items.RemoveAt(index);
                inventory.itemObjects.RemoveAt(index);
                inventory.itemSprites.RemoveAt(index);
                inventory.displayNames.RemoveAt(index);

                // Optionally, deactivate or destroy the associated GameObject if required
                GameObject itemObject = inventory.itemObjects[index];
                if (itemObject != null)
                {
                    itemObject.SetActive(true); // You can destroy it if needed: Destroy(itemObject);
                }
                Debug.Log(itemName + " has been completely removed from the inventory.");
            }
        }
    }
}