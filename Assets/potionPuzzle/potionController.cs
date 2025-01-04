using UnityEngine;

public class PotionController : MonoBehaviour
{
    public GameObject potion;  
    public potionManager potionManager; // Reference to the PotionManager, assign in Inspector
    private Outline outline; // Reference to the Outline component
    private Animator potionAnimator; // Cache the Animator component
    private bool isPotionOn = false; // Tracks the state of the potion
    public AudioClip bubbleSound; // Sound for solved puzzle

    private AudioSource audioSource; // Audio source

    private void Start()
    {
                audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Ensure the PotionManager is assigned
        if (potionManager == null)
        {
            Debug.LogError($"PotionManager is not assigned to the PotionController on {gameObject.name}");
        }

        // Get the Outline component
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogError($"Outline component is missing on {gameObject.name}");
        }
        else
        {
            outline.enabled = false; // Disable outline initially
        }

        // Ensure the Animator is assigned
        if (potion == null)
        {
            Debug.LogError($"Potion GameObject is not assigned to the PotionController on {gameObject.name}");
        }
        else
        {
            potionAnimator = potion.GetComponent<Animator>();
            if (potionAnimator == null)
            {
                Debug.LogError($"Animator component is missing on the potion GameObject assigned to {gameObject.name}");
            }
        }
    }

    // Function to toggle the potion animation state
    public void TogglePotionAnimation()
    {
        if (potionAnimator != null)
        {
            isPotionOn = true; // Toggle the state
            potionAnimator.SetBool("IsPotionOn", isPotionOn); // Set the Animator parameter
        }
    }

        public void OnPotionAnimationComplete()
    {
        isPotionOn = false;
        potionAnimator.SetBool("IsPotionOn", isPotionOn);
    }

    // Handle click interaction
    private void OnMouseDown()
    {
        if (potionManager == null || potionManager.IsPuzzleSolved()) return;

        // Find the index of this potion in the PotionManager's array
        int potionIndex = System.Array.IndexOf(potionManager.potions, this);
        if (potionIndex != -1)
        {
            potionManager.PotionPressed(potionIndex);
            TogglePotionAnimation(); // Toggle the potion animation state
        }
    }

    // Enable outline on mouse hover
    private void OnMouseEnter()
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    // Disable outline when the mouse leaves
    private void OnMouseExit()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    private void PlayBubbleSound()
    {
        PlaySound(bubbleSound); // Play sound for puzzle solved
    }

        private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
