using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<string> items = new List<string>(); // List of item names
    public List<GameObject> itemObjects = new List<GameObject>(); // List of item 3D objects
    public List<Sprite> itemSprites = new List<Sprite>(); // List of item sprites
    public List<string> displayNames = new List<string>(); // List of display names
    public GameObject inventoryUI; // Reference to the UI inventory
    public GameObject itemSlotPrefab; // Reference to the item slot prefab
    public Transform itemsParent; // Parent transform for the items

    private bool isInventoryOpen = false;

    void Start()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false); // Hide the UI inventory initially
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void AddItem(string itemName, GameObject itemObject, Sprite itemSprite, string displayName)
    {
        // Check if the item is already in the inventory
        if (!items.Contains(itemName))
        {
            items.Add(itemName);
            itemObjects.Add(itemObject);
            itemSprites.Add(itemSprite);
            displayNames.Add(displayName); // Add displayName
            itemObject.SetActive(false); // Deactivate the item object in the scene
            Debug.Log($"Item '{itemName}' added to inventory.");
            UpdateInventoryUI();
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' is already in the inventory.");
        }
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            UpdateInventoryUI();
        }
    }

    private void UpdateInventoryUI()
    {
        // Clear existing items in the inventory UI
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        // Add new items to the inventory UI
        for (int i = 0; i < items.Count; i++)
        {
            GameObject newItemSlot = Instantiate(itemSlotPrefab, itemsParent); // Create a new slot

            // Find the Image component to set the item sprite
            Image image = newItemSlot.GetComponentInChildren<Image>();
            if (image != null)
            {
                image.sprite = itemSprites[i]; // Set the sprite of the item
                image.enabled = true; // Ensure the image is enabled
            }
            else
            {
                Debug.LogWarning("Image component not found in item slot prefab.");
            }

            // Find the TMP_Text component to set the item name (displayName)
            TMP_Text itemText = newItemSlot.GetComponentInChildren<TMP_Text>();
            if (itemText != null)
            {
                itemText.text = displayNames[i]; // Set the display name of the item
            }
            else
            {
                Debug.LogWarning("TMP_Text component not found in item slot prefab.");
            }
        }
    }

    public void RemoveItem(string itemName)
    {
        // Search for the index of the item to remove
        int index = items.IndexOf(itemName);

        if (index >= 0)
        {
            // Remove the item from all the lists (items, itemObjects, itemSprites, displayNames)
            GameObject removedItemObject = itemObjects[index]; // Store the item object before removing

            items.RemoveAt(index); // Remove from items list
            itemObjects.RemoveAt(index); // Remove from itemObjects list
            itemSprites.RemoveAt(index); // Remove from itemSprites list
            displayNames.RemoveAt(index); // Remove from displayNames list

            // Optionally, reactivate or destroy the associated GameObject
            if (removedItemObject != null)
            {
                removedItemObject.SetActive(true); // Reactivate the item object in the scene if needed
                                                   // Or, you can destroy it: Destroy(removedItemObject);
            }

            Debug.Log($"Item '{itemName}' has been removed from inventory.");
            UpdateInventoryUI(); // Update the UI to reflect the changes
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' not found in inventory.");
        }
    }
}