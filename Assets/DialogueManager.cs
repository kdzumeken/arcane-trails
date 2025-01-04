using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public Wizard wizard;
    public DialogueTree dialogueTree;
    public GameObject dialogueUI;
    public Text dialogueText;
    public Button[] optionButtons;
    public GameObject door;
    public Text debugText;

    public AudioClip[] dialogueAudioClips; // Array of MP3s to play
    private AudioSource audioSource; // AudioSource component to play the MP3s

    private int currentNodeIndex = 0;
    private bool hasTriggeredFinalDialogue = false;
    private int currentAudioIndex = 0; // To track the index of the MP3s

    void Start()
    {
        dialogueUI.SetActive(false);
        ValidateDialogueTree();

        // Get or add an AudioSource component to play audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void StartDialogue()
    {
        if (hasTriggeredFinalDialogue)
        {
            Debug.Log("Final dialogue has already been triggered. Cannot start new dialogue.");
            return;
        }

        wizard.EnablePointerMode();
        currentNodeIndex = 0;
        ShowDialogue();
    }

public void ShowDialogue()
{
    if (currentNodeIndex < 0 || currentNodeIndex >= dialogueTree.nodes.Length)
    {
        Debug.LogError($"Invalid currentNodeIndex: {currentNodeIndex}. Dialogue aborted.");
        EndDialogue();
        return;
    }

    DialogueNode currentNode = dialogueTree.nodes[currentNodeIndex];
    Debug.Log($"Displaying Dialogue ID: {currentNodeIndex}, Text: {currentNode.dialogueText}");

    dialogueUI.SetActive(true);
    dialogueText.text = currentNode.dialogueText;

    // Play the corresponding MP3 based on the node index
    PlayAudioClipForNode(currentNodeIndex);

    // Update debug information
    UpdateDebugPanel(currentNode);

    for (int i = 0; i < optionButtons.Length; i++)
    {
        if (i < currentNode.options.Length)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<Text>().text = currentNode.options[i];

            // Assign OnClick listener dynamically
            int nextIndex = currentNode.nextNodeIndexes[i];
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => ChooseOption(nextIndex));
        }
        else
        {
            optionButtons[i].gameObject.SetActive(false);
        }
    }
}


    public void ChooseOption(int nextNodeIndex)
{
    Debug.Log($"Selected Option -> Next Dialogue ID: {nextNodeIndex}");

    if (nextNodeIndex < 0 || nextNodeIndex >= dialogueTree.nodes.Length)
    {
        Debug.LogError($"Invalid nextNodeIndex: {nextNodeIndex}. Check your dialogue tree.");
        EndDialogue();
        return;
    }

    // Wait for 0.5 seconds before showing the next dialogue
    StartCoroutine(WaitAndShowNextDialogue(nextNodeIndex));
}

private IEnumerator WaitAndShowNextDialogue(int nextNodeIndex)
{
    yield return new WaitForSeconds(0.5f);

    DialogueNode nextNode = dialogueTree.nodes[nextNodeIndex];
    if (nextNode.isFailure)
    {
        Debug.Log("Failure dialogue reached. Ending dialogue.");
        EndDialogue();
        wizard.DisablePointerMode();
    }
    else if (nextNode.isFinal)
    {
        Debug.Log("Final dialogue reached. Opening door.");
        OpenDoor();
        hasTriggeredFinalDialogue = true;
        wizard.DisablePointerMode();
    }
    else
    {
        currentNodeIndex = nextNodeIndex;
        ShowDialogue();
    }
}

    public void EndDialogue()
    {
        Debug.Log("Dialogue ended.");
        dialogueUI.SetActive(false);
    }

    public void OpenDoor()
    {
        Debug.Log("Opening door via animation trigger.");
        dialogueUI.SetActive(false);
        door.GetComponent<Animator>().SetTrigger("Open");
    }

    private void ValidateDialogueTree()
    {
        for (int i = 0; i < dialogueTree.nodes.Length; i++)
        {
            DialogueNode node = dialogueTree.nodes[i];
            for (int j = 0; j < node.nextNodeIndexes.Length; j++)
            {
                int nextIndex = node.nextNodeIndexes[j];
                if (nextIndex < 0 || nextIndex >= dialogueTree.nodes.Length)
                {
                    Debug.LogError($"Node ID {i} has invalid NextNodeIndex: {nextIndex}. Check your dialogue data.");
                }
            }
        }
        Debug.Log("Dialogue tree validation complete.");
    }

    private void UpdateDebugPanel(DialogueNode currentNode)
    {
        if (debugText != null)
        {
            debugText.text = $"Current Node ID: {currentNodeIndex}\n" +
                             $"Dialogue: {currentNode.dialogueText}\n" +
                             $"Options:\n";

            for (int i = 0; i < currentNode.options.Length; i++)
            {
                debugText.text += $"- {currentNode.options[i]} (Leads to Node ID: {currentNode.nextNodeIndexes[i]})\n";
            }
        }
    }

    private void PlayAudioClipForNode(int nodeIndex)
    {
        if (nodeIndex < 0 || nodeIndex >= dialogueAudioClips.Length)
        {
            Debug.LogWarning("Audio index is out of range or not mapped for this node.");
            return;
        }

        audioSource.clip = dialogueAudioClips[nodeIndex];
        audioSource.Play();
    }

    public bool HasTriggeredFinalDialogue()
    {
        return hasTriggeredFinalDialogue;
    }

    
}
