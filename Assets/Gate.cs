using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public bool IsLocked = false;
    public bool GateClosed = true;
    public float OpenRotationAmount = 90f; // Sudut rotasi untuk membuka daun pagar
    public float RotationSpeed = 1f;
    public float MaxDistance = 3.0f;
    public string playerTag = "Player";

    public Transform LeftGate;  // Referensi ke Pintu Kiri
    public Transform RightGate; // Referensi ke Pintu Kanan

    private GameObject Player;
    private Camera Cam;
    private bool Rotating = false;

    private float CurrentLerpTime = 0f;
    private float LerpTime = 1f;

    private Quaternion LeftGateStartRotation;
    private Quaternion RightGateStartRotation;

    private Quaternion LeftGateEndRotation;
    private Quaternion RightGateEndRotation;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag(playerTag);

        if (!Player)
        {
            Debug.LogWarning(this.GetType().Name + ".cs on " + this.name + ", No object tagged with " + playerTag + " found in Scene", gameObject);
            return;
        }

        Cam = Camera.main;

        LeftGateStartRotation = LeftGate.localRotation;
        RightGateStartRotation = RightGate.localRotation;
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
        Vector3 midpoint = (LeftGate.position + RightGate.position) / 2;
        if (Vector3.Distance(midpoint, Player.transform.position) <= MaxDistance)
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
        LeftGate.localRotation = Quaternion.Lerp(LeftGateStartRotation, LeftGateEndRotation, _Perc);
        RightGate.localRotation = Quaternion.Lerp(RightGateStartRotation, RightGateEndRotation, _Perc);

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
        LeftGateEndRotation = LeftGateStartRotation * Quaternion.Euler(0, -OpenRotationAmount, 0);  // Pintu kiri membuka ke kiri
        RightGateEndRotation = RightGateStartRotation * Quaternion.Euler(0, OpenRotationAmount, 0); // Pintu kanan membuka ke kanan

        Rotating = true;
    }

    void CloseGates()
    {
        GateClosed = true;
        CurrentLerpTime = 0;

        // Kembali ke rotasi awal saat menutup
        LeftGateEndRotation = LeftGateStartRotation;
        RightGateEndRotation = RightGateStartRotation;

        Rotating = true;
    }
}