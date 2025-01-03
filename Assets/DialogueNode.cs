using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    public string dialogueText;
    public string[] options; // Array for 3 options
    public int[] nextNodeIndexes; // Array of indexes for next dialogue nodes
    public bool isFinal; // If true, opens the door
    public bool isFailure; // If true, ends the dialogue system
}
