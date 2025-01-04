using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SunTemple
{
    public class Bookshelf : MonoBehaviour
    {
        public bool IsLocked = false;
        public bool DoorClosed = true;
        public float OpenSideAmount = 3.0f; // Amount to move the gate sideways
        public float MoveSpeed = 1f;
        public string playerTag = "Player";
        public string requiredItem = "Key"; // Item required to unlock the gate
        public List<GameObject> gates; // List of gate objects
        public Collider bookshelfCollider; // Collider used to detect player

        private GameObject Player;
        private Camera Cam;
        private CursorManager cursor;

        Vector3[] StartPositions;
        Vector3[] EndPositions;
        float LerpTime = 1f;
        float CurrentLerpTime = 0;
        bool Moving;

        private bool scriptIsEnabled = true;
        private bool playerInRange = false;

        // UI Elements
        public TextMeshProUGUI interactionText;
        public TextMeshProUGUI lockedText;

        void Start()
        {
            if (gates == null || gates.Count == 0)
            {
                Debug.LogWarning(this.GetType().Name + ".cs on " + gameObject.name + " has no gates assigned", gameObject);
                scriptIsEnabled = false;
                return;
            }

            StartPositions = new Vector3[gates.Count];
            EndPositions = new Vector3[gates.Count];

            for (int i = 0; i < gates.Count; i++)
            {
                StartPositions[i] = gates[i].transform.localPosition;
                EndPositions[i] = StartPositions[i] + new Vector3(OpenSideAmount, 0, 0);
            }

            Player = GameObject.FindGameObjectWithTag(playerTag);

            if (!Player)
            {
                Debug.LogWarning(this.GetType().Name + ".cs on " + this.name + ", No object tagged with " + playerTag + " found in Scene", gameObject);
                scriptIsEnabled = false;
                return;
            }

            Cam = Camera.main;
            if (!Cam)
            {
                Debug.LogWarning(this.GetType().Name + ", No objects tagged with MainCamera in Scene", gameObject);
                scriptIsEnabled = false;
            }

            cursor = CursorManager.instance;

            if (cursor != null)
            {
                cursor.SetCursorToDefault();
            }

            // Hide UI elements initially
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }

            if (lockedText != null)
            {
                lockedText.gameObject.SetActive(false);
            }

            // Check if bookshelfCollider is assigned
            if (bookshelfCollider == null)
            {
                Debug.LogWarning(this.GetType().Name + ".cs on " + gameObject.name + " has no Collider assigned", gameObject);
                scriptIsEnabled = false;
            }
        }

        void Update()
        {
            if (scriptIsEnabled)
            {
                if (Moving)
                {
                    Move();
                }

                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    TryToOpen();
                }

                if (cursor != null)
                {
                    CursorHint();
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (bookshelfCollider.enabled && other.CompareTag(playerTag))
            {
                playerInRange = true;
                if (interactionText != null)
                {
                    interactionText.gameObject.SetActive(true);
                    interactionText.text = "???????????";
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInRange = false;
                if (interactionText != null)
                {
                    interactionText.gameObject.SetActive(false);
                }
                if (lockedText != null)
                {
                    lockedText.gameObject.SetActive(false);
                }
                if (cursor != null)
                {
                    cursor.SetCursorToDefault();
                }
            }
        }

        void TryToOpen()
        {
            if (!bookshelfCollider.enabled) return;

            Inventory inventory = Player.GetComponent<Inventory>();
            if (inventory != null && inventory.items.Contains(requiredItem))
            {
                if (IsLocked == false)
                {
                    Activate();
                    RemoveItemAndAllRelated(inventory, requiredItem); // Remove the item and its related elements from the inventory
                    Debug.Log(requiredItem + " has been used and removed from the inventory.");
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
                    if (bookshelfCollider != null)
                    {
                        bookshelfCollider.enabled = false;
                    }
                    // Disable colliders on all gate objects
                    foreach (var gate in gates)
                    {
                        Collider gateObjCollider = gate.GetComponent<Collider>();
                        if (gateObjCollider != null)
                        {
                            gateObjCollider.enabled = false;
                        }
                    }
                }
            }
            else
            {
                if (lockedText != null)
                {
                    lockedText.text = $"It won't budge. It seems like it need something to open it.";
                    StartCoroutine(ShowLockedText());
                }
                Debug.Log("You need a " + requiredItem + " to open this gate.");
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

        void CursorHint()
        {
            if (playerInRange)
            {
                if (IsLocked == false)
                {
                    cursor.SetCursorToDoor();
                }
                else if (IsLocked == true)
                {
                    cursor.SetCursorToLocked();
                }
            }
        }

        public void Activate()
        {
            if (DoorClosed)
                Open();
            else
                Close();
        }

        void Move()
        {
            CurrentLerpTime += Time.deltaTime * MoveSpeed;
            if (CurrentLerpTime > LerpTime)
            {
                CurrentLerpTime = LerpTime;
            }

            float _Perc = CurrentLerpTime / LerpTime;

            for (int i = 0; i < gates.Count; i++)
            {
                gates[i].transform.localPosition = Vector3.Lerp(StartPositions[i], EndPositions[i], _Perc);
            }

            if (CurrentLerpTime == LerpTime)
            {
                Moving = false;
            }
        }

        void Open()
        {
            DoorClosed = false;
            CurrentLerpTime = 0;
            Moving = true;

            // Disable BoxCollider when the door is open
            if (bookshelfCollider != null)
            {
                bookshelfCollider.enabled = false;
            }
        }

        void Close()
        {
            DoorClosed = true;
            CurrentLerpTime = 0;
            Moving = true;

            // Enable BoxCollider when the door is closed
            if (bookshelfCollider != null)
            {
                bookshelfCollider.enabled = true;
            }
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
                    itemObject.SetActive(false); // You can destroy it if needed: Destroy(itemObject);
                }
                Debug.Log(itemName + " has been completely removed from the inventory.");
            }
        }
    }
}
