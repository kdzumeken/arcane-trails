using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> items = new List<string>();

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log($"Item '{itemName}' ditambahkan ke inventory.");
    }

    public void ShowInventory() 
    {
        Debug.Log("Inventory:");
        foreach (string item in items)
        {
            Debug.Log($"- {item}");
        }
    }
}
