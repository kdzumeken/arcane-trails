using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public GameObject door;
    public Outline outline; // Reference to the Outline component

    void Start()
    {
        // Ensure the outline is initially disabled
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 5f)
        {
            // Only trigger dialogue if final dialogue has not been triggered yet
            if (!dialogueManager.HasTriggeredFinalDialogue())
            {
                dialogueManager.StartDialogue();
                dialogueManager.dialogueUI.SetActive(true);
            }
            else
            {
                Debug.Log("Final dialogue has already been triggered, door interaction blocked.");
            }
        }
    }

    void OnMouseEnter()
    {
        // Enable the outline when the mouse enters the object
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    void OnMouseExit()
    {
        // Disable the outline when the mouse exits the object
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    public void OpenDoor()
    {
        door.GetComponent<Animator>().SetTrigger("Open");
    }
}
