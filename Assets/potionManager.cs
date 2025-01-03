using UnityEngine;
using System.Collections.Generic;

public class potionManager : MonoBehaviour
{
    public PotionController[] potions; // Array of potion controllers
    
    public GameObject chest; // Chest that will open
    public List<int> correctSequence = new List<int>(); // Correct sequence, set in Inspector
    private List<int> playerSequence = new List<int>(); // Player's input sequence
    private bool puzzleSolved = false; // Status of whether the puzzle is solved

    public AudioClip potionPressSound; // Sound when potion is pressed
    public AudioClip wrongSequenceSound; // Sound for incorrect sequence
    public AudioClip puzzleSolvedSound; // Sound for solved puzzle
    private AudioSource audioSource; // Audio source

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Function to handle potion press
    public void PotionPressed(int potionIndex)
    {
        if (puzzleSolved) return; // Prevent further interaction if puzzle is solved

        // Add player's input to sequence
        playerSequence.Add(potionIndex);

        // Trigger animation for the pressed potion
        potions[potionIndex].TogglePotionAnimation();

        // Play potion press sound
        PlaySound(potionPressSound);

        // Check if the player's sequence is complete
        if (playerSequence.Count == correctSequence.Count)
        {
            CheckSequence();
        }
    }

    // Function to check if the sequence is correct
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
            PlaySound(wrongSequenceSound); // Play sound for incorrect sequence
            Invoke("ResetPuzzle", 1.0f); // Reset puzzle after a delay
        }
    }

    // Function to open the chest when sequence is correct
    private void OpenChest()
    {
        puzzleSolved = true; // Mark puzzle as solved
        Destroy(chest); // Destroy chest object (simulate opening)
        PlaySound(puzzleSolvedSound); // Play sound for puzzle solved
        Debug.Log("Chest opened!");
    }

    // Function to reset puzzle when sequence is incorrect
    private void ResetPuzzle()
    {
        playerSequence.Clear(); // Clear player's input sequence
        Debug.Log("Incorrect sequence! Puzzle reset.");
    }

    // Function to check if puzzle is solved
    public bool IsPuzzleSolved()
    {
        return puzzleSolved;
    }

    // Function to play sounds
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
