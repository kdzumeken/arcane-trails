using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 5f)
        {
            dialogueManager.StartDialogue();
            dialogueManager.dialogueUI.SetActive(true);
        }
    }
}
