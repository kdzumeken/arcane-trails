using UnityEngine;
using System.Collections.Generic;

namespace SunTemple
{
    public class PuzzleKimia : MonoBehaviour
    {
        // Ganti tipe data menjadi List<string> agar bisa memasukkan nama item (string) di Inspector
        public List<string> chemicalBottles = new List<string>(); // Daftar nama botol kimia
        public List<string> correctSequence = new List<string>(); // Urutan yang benar berdasarkan nama item
        private List<string> playerSequence = new List<string>(); // Urutan yang dimasukkan pemain
        private bool puzzleSolved = false; // Status apakah puzzle sudah terpecahkan

        public AudioClip bottlePlaceSound; // Suara saat botol ditempatkan
        public AudioClip wrongSequenceSound; // Suara saat urutan salah
        public AudioClip puzzleSolvedSound; // Suara saat puzzle terpecahkan
        private AudioSource audioSource; // Sumber audio

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Fungsi untuk menangani item yang ditempatkan di kuali
        public void ItemPlaced(string itemName)
        {
            if (puzzleSolved) return; // Jika puzzle sudah terpecahkan, tidak bisa menempatkan botol

            // Menambahkan item ke urutan pemain
            playerSequence.Add(itemName);

            // Mainkan suara saat botol ditempatkan
            PlaySound(bottlePlaceSound);

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
                PlaySound(wrongSequenceSound); // Mainkan suara urutan salah
                Invoke("ResetPuzzle", 1.0f); // Reset puzzle after a delay
            }
        }

        // Fungsi untuk membuka peti jika urutan benar
        private void OpenChest()
        {
            puzzleSolved = true; // Menandai puzzle sudah terpecahkan
            PlaySound(puzzleSolvedSound); // Mainkan suara puzzle terpecahkan
            Debug.Log("Peti terbuka!");
        }

        // Fungsi untuk mereset puzzle jika urutan salah
        private void ResetPuzzle()
        {
            playerSequence.Clear(); // Menghapus urutan input pemain
            Debug.Log("Urutan salah! Reset puzzle.");
        }

        // Fungsi untuk memainkan suara
        private void PlaySound(AudioClip clip)
        {
            if (clip != null && audioSource != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
