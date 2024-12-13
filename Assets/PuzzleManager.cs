using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public LampController[] lamps; // Array lampu-lampu
    public GameObject chest; // Peti yang akan terbuka
    public List<int> correctSequence = new List<int>(); // Urutan yang benar, bisa diatur di Inspector
    private List<int> playerSequence = new List<int>(); // Urutan yang dimasukkan pemain
    private bool puzzleSolved = false; // Status apakah puzzle sudah terpecahkan

    // Fungsi untuk menekan tombol pada lampu
    public void LampPressed(int lampIndex)
    {
        if (puzzleSolved) return; // Jika puzzle sudah terpecahkan, tidak bisa menekan lampu

        // Menambahkan input pemain ke urutan
        playerSequence.Add(lampIndex);

        // Menyalakan lampu yang sesuai
        lamps[lampIndex].ToggleLamp();

        // Periksa apakah urutan pemain sudah lengkap
        if (playerSequence.Count == correctSequence.Count)
        {
            CheckSequence();
        }
    }

    // Fungsi untuk memeriksa apakah urutan input benar
    private void CheckSequence()
    {
        bool isCorrect = true;
        for (int i = 0; i < playerSequence.Count; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            OpenChest();
        }
        else
        {
            Invoke("ResetPuzzle", 1.0f); // Reset puzzle after a delay
        }
    }

    // Fungsi untuk membuka peti jika urutan benar
    private void OpenChest()
    {
        puzzleSolved = true; // Menandai bahwa puzzle sudah terpecahkan
        Destroy(chest); // Menghancurkan peti (anggap peti terbuka)
        Debug.Log("Peti terbuka!");
    }

    // Fungsi untuk mereset puzzle jika urutan salah
    private void ResetPuzzle()
    {
        foreach (var lamp in lamps)
        {
            if (lamp.IsOn())
            {
                lamp.ToggleLamp(); // Mematikan semua lampu yang menyala
            }
        }

        playerSequence.Clear(); // Menghapus urutan input pemain
        Debug.Log("Urutan salah! Reset puzzle.");
    }

    // Fungsi untuk memeriksa apakah puzzle sudah terpecahkan
    public bool IsPuzzleSolved()
    {
        return puzzleSolved;
    }
}
