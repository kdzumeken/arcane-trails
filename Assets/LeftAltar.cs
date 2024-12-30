using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SunTemple
{
    public class LeftAltar : MonoBehaviour
    {
        public string requiredItem = "GreenApple"; // Item required for the left altar
        public Transform itemPosition; // Position where the item will be placed
        private bool itemPlaced = false;
        private bool playerInRange = false;

        // UI Elements
        public TextMeshProUGUI interactionText;
        public TextMeshProUGUI lockedText;

        // Collider
        public Collider altarCollider; // Collider used to detect player

        public bool IsItemPlaced()
        {
            return itemPlaced;
        }

        private void Start()
        {
            // Ensure UI elements are hidden initially
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }

            if (lockedText != null)
            {
                lockedText.gameObject.SetActive(false);
            }

            // Check if altarCollider is assigned
            if (altarCollider == null)
            {
                Debug.LogWarning(this.GetType().Name + ".cs on " + gameObject.name + " has no Collider assigned", gameObject);
            }
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
                if (interactionText != null)
                {
                    interactionText.gameObject.SetActive(true);
                    interactionText.text = "Press 'E' to place Green Apple";
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player exited left altar trigger area.");
                playerInRange = false;
                if (interactionText != null)
                {
                    interactionText.gameObject.SetActive(false);
                }
                if (lockedText != null)
                {
                    lockedText.gameObject.SetActive(false);
                }
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
                    Debug.Log(requiredItem + " has been placed on the left altar.");

                    // After item is placed, remove it and its associated elements
                    RemoveItemAndAllRelated(inventory, requiredItem);

                    itemPlaced = true;

                    // Hide interactionText and lockedText permanently
                    if (interactionText != null)
                    {
                        interactionText.gameObject.SetActive(false);
                    }
                    if (lockedText != null)
                    {
                        lockedText.gameObject.SetActive(false);
                    }
                    // Disable the collider to prevent further triggers
                    if (altarCollider != null)
                    {
                        altarCollider.enabled = false;
                    }
                }
                else
                {
                    if (lockedText != null)
                    {
                        lockedText.text = $"{requiredItem} not found in inventory.";
                        StartCoroutine(ShowLockedText());
                    }
                    Debug.Log("Required item " + requiredItem + " not found in inventory.");
                }
            }
            else
            {
                Debug.Log("Inventory component not found on player.");
            }
        }

        IEnumerator ShowLockedText()
        {
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
            if (lockedText != null)
            {
                lockedText.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(3);
            if (lockedText != null)
            {
                lockedText.gameObject.SetActive(false);
            }
            if (interactionText != null && playerInRange)
            {
                interactionText.gameObject.SetActive(true);
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
