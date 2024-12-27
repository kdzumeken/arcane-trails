using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;

    public GameObject door;

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

    public void OpenDoor()
    {
        door.GetComponent<Animator>().SetTrigger("Open");
    }
}
