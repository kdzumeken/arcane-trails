using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{
    public class LockedGate : MonoBehaviour
    {
        public bool IsLocked = false;
        public bool DoorClosed = true;
        public float OpenHeightAmount = 3.0f; // Amount to move the gate upwards
        public float MoveSpeed = 1f;
        public string playerTag = "Player";
        public string requiredItem = "Key"; // Item required to unlock the gate
        public List<GameObject> gates; // List of gate objects

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
                EndPositions[i] = StartPositions[i] + new Vector3(0, OpenHeightAmount, 0);
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
            if (other.CompareTag(playerTag))
            {
                playerInRange = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInRange = false;
                if (cursor != null)
                {
                    cursor.SetCursorToDefault();
                }
            }
        }

        void TryToOpen()
        {
            Inventory inventory = Player.GetComponent<Inventory>();
            if (inventory != null && inventory.items.Contains(requiredItem))
            {
                if (IsLocked == false)
                {
                    Activate();
                    // Remove the used item and all other items from the inventory
                    RemoveItemAndAllRelated(inventory, requiredItem);
                    Debug.Log(requiredItem + " has been used and removed from the inventory.");
                }
            }
            else
            {
                Debug.Log("You need a " + requiredItem + " to open this gate.");
            }
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
        }

        void Close()
        {
            DoorClosed = true;
            CurrentLerpTime = 0;
            Moving = true;
        }
    }
}
