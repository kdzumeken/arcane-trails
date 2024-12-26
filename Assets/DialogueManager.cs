using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public Wizard wizard;
    public DialogueTree dialogueTree;
    public GameObject dialogueUI;
    public Text dialogueText;
    public Button[] optionButtons;
    public GameObject door;

    private int currentNodeIndex = 0;

    void Start()
    {
        dialogueUI.SetActive(false);
    }

    public void StartDialogue()
    {
        wizard.EnablePointerMode();
        currentNodeIndex = 0; // Reset to start
        ShowDialogue();
    }

public void ShowDialogue()
{
    DialogueNode currentNode = dialogueTree.nodes[currentNodeIndex];
    dialogueText.text = currentNode.dialogueText;

    for (int i = 0; i < optionButtons.Length; i++)
    {
        if (i < currentNode.options.Length)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<Text>().text = currentNode.options[i];

            // Assign OnClick listener dynamically
            int nextIndex = currentNode.nextNodeIndexes[i];
            optionButtons[i].onClick.RemoveAllListeners(); // Clear previous listeners
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
        DialogueNode nextNode = dialogueTree.nodes[nextNodeIndex];
        if (nextNode.isFailure)
        {
            EndDialogue();
        }
        else if (nextNode.isFinal)
        {
            OpenDoor();
        }
        else
        {
            currentNodeIndex = nextNodeIndex;
            ShowDialogue();
        }
    }

    public void EndDialogue()
    {
        wizard.DisablePointerMode();
        dialogueUI.SetActive(false);
    }

    public void OpenDoor()
    {
        dialogueUI.SetActive(false);
        door.GetComponent<Animator>().SetTrigger("Open"); // Requires an Animator on the door
    }
}
