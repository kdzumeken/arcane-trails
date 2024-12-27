using UnityEngine;

[CreateAssetMenu(fileName = "DialogueTree", menuName = "ScriptableObjects/DialogueTree", order = 1)]
public class DialogueTree : ScriptableObject
{
    public DialogueNode[] nodes;
}
