using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SunTemple
{
    public class AltarBall : MonoBehaviour
    {
        public List<string> requiredItems = new List<string> { "Statue", "Gem", "Amulet" };
        public Transform itemPosition; // Position where items will be placed
        private HashSet<string> placedItems = new HashSet<string>();
        private bool playerInRange = false;

        public TextMeshProUGUI interactionText;
        public TextMeshProUGUI lockedText;
        public Collider altarCollider;

        public bool AreAllItemsPlaced()
        {
            return placedItems.Count == requiredItems.Count;
        }

        private void Start()
        {
            interactionText?.gameObject.SetActive(false);
            lockedText?.gameObject.SetActive(false);

            if (altarCollider == null)
            {
                Debug.LogWarning($"{this.GetType().Name} on {gameObject.name} has no Collider assigned", gameObject);
            }
        }

        void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                TryToPlaceAllItems();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                interactionText?.gameObject.SetActive(true);
                interactionText.text = "Press 'E' to place items";
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                interactionText?.gameObject.SetActive(false);
                lockedText?.gameObject.SetActive(false);
            }
        }

        void TryToPlaceAllItems()
        {
            Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            if (inventory != null)
            {
                bool itemPlaced = false;
                foreach (string item in requiredItems)
                {
                    if (inventory.items.Contains(item) && !placedItems.Contains(item))
                    {
                        RemoveItemAndAllRelated(inventory, item);
                        placedItems.Add(item);
                        GameObject itemObject = new GameObject(item);
                        itemObject.transform.position = itemPosition.position;
                        itemPlaced = true;
                        Debug.Log(item + " placed on the altar.");

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
                }

                if (!itemPlaced && lockedText != null)
                {
                    lockedText.text = "Required items not found in inventory.";
                    StartCoroutine(ShowLockedText());
                }
            }
            else
            {
                Debug.Log("Inventory component not found on player.");
            }
        }

        IEnumerator ShowLockedText()
        {
            interactionText?.gameObject.SetActive(false);
            lockedText?.gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            lockedText?.gameObject.SetActive(false);
            if (playerInRange) interactionText?.gameObject.SetActive(true);
        }

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
