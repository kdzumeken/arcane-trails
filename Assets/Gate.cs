using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SunTemple
{
    public class Gate : MonoBehaviour
    {
        public bool IsLocked = false;
        public bool DoorClosed = true;
        public float OpenRotationAmount = 90.0f; // Amount to rotate the gate
        public float MoveSpeed = 1f;
        public string playerTag = "Player";
        public string requiredItem = "Key"; // Item required to unlock the gate
        public List<GameObject> gates; // List of gate objects
        public Collider gateCollider; // Collider used to detect player

        private GameObject Player;
        private Camera Cam;
        private CursorManager cursor;

        Quaternion[] StartRotations;
        Quaternion[] EndRotations;
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

            StartRotations = new Quaternion[gates.Count];
            EndRotations = new Quaternion[gates.Count];

            for (int i = 0; i < gates.Count; i++)
            {
                StartRotations[i] = gates[i].transform.localRotation;
                EndRotations[i] = StartRotations[i] * Quaternion.Euler(0, OpenRotationAmount, 0);
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

            // Check if gateCollider is assigned
            if (gateCollider == null)
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
            if (gateCollider.enabled && other.CompareTag(playerTag))
            {
                playerInRange = true;
                if (interactionText != null)
                {
                    interactionText.gameObject.SetActive(true);
                    interactionText.text = "Press 'E' to open";
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
            if (!gateCollider.enabled) return;

            Inventory inventory = Player.GetComponent<Inventory>();
            if (inventory != null && inventory.items.Contains(requiredItem))
            {
                if (IsLocked == false)
                {
                    Activate();
                    // Remove the used item and all other items from the inventory
                    RemoveItemAndAllRelated(inventory, requiredItem);
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
                    if (gateCollider != null)
                    {
                        gateCollider.enabled = false;
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
                    lockedText.text = $"Gate is Locked. {requiredItem} needed.";
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

            // Ensure UI elements are disabled after opening the gate
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
            if (lockedText != null)
            {
                lockedText.gameObject.SetActive(false);
            }

            // Ensure the gate collider on this GameObject is disabled
            if (gateCollider != null)
            {
                gateCollider.enabled = false;
            }
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
                gates[i].transform.localRotation = Quaternion.Lerp(StartRotations[i], EndRotations[i], _Perc);
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
        }

        void Close()
        {
            DoorClosed = true;
            CurrentLerpTime = 0;
            Moving = true;
        }

        // Function to remove the specified item and all related data from the inventory
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
