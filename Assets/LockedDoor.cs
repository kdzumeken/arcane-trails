using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{
    public class LockedDoor : MonoBehaviour
    {
        public bool IsLocked = true; // Default to locked
        public bool DoorClosed = true;
        public float OpenRotationAmount = 90;
        public float RotationSpeed = 1f;
        public float MaxDistance = 3.0f;
        public string playerTag = "Player";
        public string requiredItem = "Key"; // Item required to unlock the door
        private Collider DoorCollider;

        private GameObject Player;
        private Camera Cam;
        private CursorManager cursor;

        Vector3 StartRotation;
        float StartAngle = 0;
        float EndAngle = 0;
        float LerpTime = 1f;
        float CurrentLerpTime = 0;
        bool Rotating;

        private bool scriptIsEnabled = true;
        private bool playerInRange = false;

        void Start()
        {
            StartRotation = transform.localEulerAngles;
            DoorCollider = GetComponent<BoxCollider>();

            if (!DoorCollider)
            {
                Debug.LogWarning(this.GetType().Name + ".cs on " + gameObject.name + " door has no collider", gameObject);
                scriptIsEnabled = false;
                return;
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
                if (Rotating)
                {
                    Rotate();
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
                if (IsLocked)
                {
                    Debug.Log("Player has the key, unlocking door.");
                    IsLocked = false;
                    Activate();
                    // Remove the used item from the inventory
                    inventory.items.Remove(requiredItem);
                    Debug.Log(requiredItem + " has been used and removed from the inventory.");
                }
                else
                {
                    Debug.Log("Door is not locked, attempting to activate.");
                    Activate();
                }
            }
            else
            {
                Debug.Log("You need a " + requiredItem + " to open this door.");
            }
        }

        void CursorHint()
        {
            if (playerInRange)
            {
                if (IsLocked)
                {
                    cursor.SetCursorToLocked();
                }
                else
                {
                    cursor.SetCursorToDoor();
                }
            }
        }

        public void Activate()
        {
            if (DoorClosed)
            {
                Debug.Log("Opening door.");
                Open();
            }
            else
            {
                Debug.Log("Closing door.");
                Close();
            }
        }

        void Rotate()
        {
            CurrentLerpTime += Time.deltaTime * RotationSpeed;
            if (CurrentLerpTime > LerpTime)
            {
                CurrentLerpTime = LerpTime;
            }

            float _Perc = CurrentLerpTime / LerpTime;

            float _Angle = CircularLerp.Clerp(StartAngle, EndAngle, _Perc);
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, _Angle, transform.eulerAngles.z);

            if (CurrentLerpTime == LerpTime)
            {
                Rotating = false;
                DoorCollider.enabled = true;
                Debug.Log("Rotation complete.");
            }
        }

        void Open()
        {
            DoorCollider.enabled = false;
            DoorClosed = false;
            StartAngle = transform.localEulerAngles.y;
            EndAngle = StartRotation.y + OpenRotationAmount;
            CurrentLerpTime = 0;
            Rotating = true;
        }

        void Close()
        {
            DoorCollider.enabled = false;
            DoorClosed = true;
            StartAngle = transform.localEulerAngles.y;
            EndAngle = transform.localEulerAngles.y - OpenRotationAmount;
            CurrentLerpTime = 0;
            Rotating = true;
        }
    }
}
