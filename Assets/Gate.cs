using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public bool IsLocked = false;
    public bool GateClosed = true;
    public float OpenRotationAmount = 90f; // Sudut rotasi untuk membuka daun pagar
    public float RotationSpeed = 1f;
    public float MaxDistance = 5.0f;
    public string playerTag = "Player";

    public Transform LeftGate;  // Referensi ke Pintu Kiri
    public Transform RightGate; // Referensi ke Pintu Kanan

    private BoxCollider LeftGateCollider;
    private BoxCollider RightGateCollider;

    private GameObject Player;
    private Camera Cam;
    private bool Rotating = false;

    private float CurrentLerpTime = 0f;
    private float LerpTime = 1f;

    private float LeftGateStartAngle;
    private float RightGateStartAngle;

    private float LeftGateEndAngle;
    private float RightGateEndAngle;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag(playerTag);

        if (!Player)
        {
            Debug.LogWarning(this.GetType().Name + ".cs on " + this.name + ", No object tagged with " + playerTag + " found in Scene", gameObject);
            return;
        }

        Cam = Camera.main;

        LeftGateStartAngle = LeftGate.localEulerAngles.y;
        RightGateStartAngle = RightGate.localEulerAngles.y;

        LeftGateCollider = LeftGate.GetComponent<BoxCollider>();
        RightGateCollider = RightGate.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (Rotating)
        {
            RotateGates(); // Buka kedua pintu secara bersamaan
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryToOpen();
        }
    }

    void TryToOpen()
    {
        Vector3 playerPosition = Player.transform.position;

        // Get the closest points on the colliders to the player
        Vector3 closestPointLeftGate = LeftGateCollider.ClosestPoint(playerPosition);
        Vector3 closestPointRightGate = RightGateCollider.ClosestPoint(playerPosition);

        // Calculate the distances from the player to the closest points
        float distanceToLeftGate = Vector3.Distance(playerPosition, closestPointLeftGate);
        float distanceToRightGate = Vector3.Distance(playerPosition, closestPointRightGate);

        // Check if the player is within the max distance of either gate
        if (distanceToLeftGate <= MaxDistance || distanceToRightGate <= MaxDistance)
        {
            if (!IsLocked)
            {
                Activate();
            }
        }
    }

    public void Activate()
    {
        if (GateClosed)
            OpenGates();
        else
            CloseGates();
    }

    void RotateGates()
    {
        CurrentLerpTime += Time.deltaTime * RotationSpeed;

        if (CurrentLerpTime > LerpTime)
        {
            CurrentLerpTime = LerpTime;
        }

        float _Perc = CurrentLerpTime / LerpTime;

        // Menggunakan Lerp untuk menginterpolasi rotasi dari posisi awal ke posisi akhir secara smooth
        float leftGateAngle = Mathf.Lerp(LeftGateStartAngle, LeftGateEndAngle, _Perc);
        float rightGateAngle = Mathf.Lerp(RightGateStartAngle, RightGateEndAngle, _Perc);

        LeftGate.localEulerAngles = new Vector3(LeftGate.localEulerAngles.x, leftGateAngle, LeftGate.localEulerAngles.z);
        RightGate.localEulerAngles = new Vector3(RightGate.localEulerAngles.x, rightGateAngle, RightGate.localEulerAngles.z);

        if (CurrentLerpTime == LerpTime)
        {
            Rotating = false;
        }
    }

    void OpenGates()
    {
        GateClosed = false;
        CurrentLerpTime = 0;

        // Tentukan rotasi akhir untuk kedua daun pintu agar terbuka bersamaan
        LeftGateEndAngle = LeftGateStartAngle + OpenRotationAmount;  // Pintu kiri membuka ke kanan
        RightGateEndAngle = RightGateStartAngle - OpenRotationAmount; // Pintu kanan membuka ke kiri

        Rotating = true;
    }

    void CloseGates()
    {
        GateClosed = true;
        CurrentLerpTime = 0;

        // Kembali ke rotasi awal saat menutup
        LeftGateEndAngle = LeftGateStartAngle;
        RightGateEndAngle = RightGateStartAngle;

        Rotating = true;
    }
}