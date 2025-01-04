using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class Monologue : MonoBehaviour
{
    [SerializeField] private NPCConversation dialogue;
    [SerializeField] private Wizard playerMovementScript; // Reference to the player's movement script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerMovementScript == null)
            {
                Debug.LogError("Player movement script is not assigned in the Inspector.");
                return;
            }

            if (dialogue == null)
            {
                Debug.LogError("Dialogue is not assigned in the Inspector.");
                return;
            }

            if (ConversationManager.Instance == null)
            {
                Debug.LogError("ConversationManager.Instance is null.");
                return;
            }

            // Disable player's movement script
            playerMovementScript.enabled = false;

            // Set dialog state to active
            playerMovementScript.SetDialogState(true);

            // Start the conversation
            ConversationManager.Instance.StartConversation(dialogue);

            // Subscribe to the conversation end event to re-enable player movement and camera movement
            ConversationManager.OnConversationEnded += OnConversationEnded;

            // Destroy this game object after the conversation starts
            Destroy(gameObject);
        }
    }

    private void OnConversationEnded()
    {
        if (playerMovementScript != null)
        {
            // Re-enable player's movement script when the conversation ends
            playerMovementScript.enabled = true;

            // Set dialog state to inactive
            playerMovementScript.SetDialogState(false);

            // Unsubscribe from the conversation end event
            ConversationManager.OnConversationEnded -= OnConversationEnded;
        }
    }
}
