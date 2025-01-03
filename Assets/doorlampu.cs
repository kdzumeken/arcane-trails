using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{
    public class doorlampu : MonoBehaviour
    {
        public bool IsLocked = false;
        public bool DoorClosed = true;
        public float OpenAngle = 90.0f; // Angle to rotate the door
        public float MoveSpeed = 1f;
        public string playerTag = "Player";
        public List<GameObject> gates; // List of gate objects
        public PuzzleManager puzzleManager; // Reference to the specific PuzzleManager

        private GameObject Player;
        private Camera Cam;
        private CursorManager cursor;

        Quaternion[] StartRotations;
        Quaternion[] EndRotations;
        float LerpTime = 1f;
        float CurrentLerpTime = 0;
        bool Moving;

        private bool scriptIsEnabled = true;

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
                EndRotations[i] = StartRotations[i] * Quaternion.Euler(0, OpenAngle, 0);
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

            if (puzzleManager == null)
            {
                Debug.LogWarning(this.GetType().Name + ".cs on " + gameObject.name + " has no PuzzleManager assigned", gameObject);
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

                if (puzzleManager.IsPuzzleSolved() && DoorClosed)
                {
                    Activate();
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
    }
}
