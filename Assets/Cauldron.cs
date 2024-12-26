using UnityEngine;
using System.Collections.Generic;

namespace SunTemple
{
    public class Cauldron : MonoBehaviour
    {
        // Daftar nama botol kimia yang dapat ditempatkan di kuali
        public List<string> chemicalBottles = new List<string>(); // Daftar nama botol kimia yang ada di inventory
        public List<string> correctSequence = new List<string>(); // Urutan yang benar berdasarkan nama item
        private List<string> playerSequence = new List<string>(); // Urutan yang dimasukkan pemain
        private bool puzzleSolved = false; // Status apakah puzzle sudah terpecahkan

        public AudioClip bottlePlaceSound; // Suara saat botol ditempatkan
        public AudioClip wrongSequenceSound; // Suara saat urutan salah
        public AudioClip puzzleSolvedSound; // Suara saat puzzle terpecahkan
        private AudioSource audioSource; // Sumber audio

        public GameObject chest; // Peti yang akan menghilang ketika puzzle berhasil

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Menyiapkan urutan yang benar dengan 5 item botol kimia
            correctSequence = new List<string> { "RedBottle", "BlueBottle", "GreenBottle", "YellowBottle", "PurpleBottle" };
        }

        // Fungsi untuk menangani item yang ditempatkan di kuali
        public void ItemPlaced(string requiredItem, Transform itemPosition)
        {
            if (puzzleSolved) return; // Jika puzzle sudah terpecahkan, tidak bisa menempatkan botol

            // Mencari inventory player
            Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

            if (inventory != null)
            {
                // Memeriksa apakah item yang dimasukkan tersedia dalam inventory
                if (inventory.items.Contains(requiredItem))
                {
                    // Menghapus item dari inventory
                    inventory.items.Remove(requiredItem);
                    Debug.Log($"{requiredItem} has been used and removed from the inventory.");

                    // Menambahkan item ke urutan pemain
                    playerSequence.Add(requiredItem);
                    Debug.Log($"{requiredItem} has been placed in the cauldron.");

                    // Mainkan suara saat botol ditempatkan
                    PlaySound(bottlePlaceSound);

                    // Menempatkan item di posisi yang diberikan
                    // Anda dapat mengatur logika untuk menempatkan item sesuai dengan posisi yang diberikan
                    // Misalnya, item ini mungkin akan muncul di posisi itemPosition, jika ada objek yang terkait
                    // atau hanya menambahkan nama item ke daftar playerSequence
                    // (karena kita menggunakan string saja, itemPosition mungkin tidak digunakan langsung)

                    // Periksa apakah urutan pemain sudah lengkap
                    if (playerSequence.Count == correctSequence.Count)
                    {
                        CheckSequence();
                    }
                }
                else
                {
                    Debug.Log($"Required item {requiredItem} not found in inventory.");
                }
            }
            else
            {
                Debug.Log("Inventory component not found on player.");
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
                Invoke("ResetPuzzle", 1.0f); // Reset puzzle setelah delay
            }
        }

        // Fungsi untuk membuka peti jika urutan benar
        private void OpenChest()
        {
            puzzleSolved = true; // Menandai puzzle sudah terpecahkan
            PlaySound(puzzleSolvedSound); // Mainkan suara puzzle terpecahkan
            Debug.Log("Peti terbuka!");
            
            // Menyembunyikan peti
            if (chest != null)
            {
                chest.SetActive(false);
            }
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
