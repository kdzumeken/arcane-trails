using UnityEngine;

namespace SunTemple
{
    public class AltarBall : MonoBehaviour
    {
        public string[] requiredItems = { "RollingBalls_Sci-fi_2_3", "RollingBalls_Sci-fi_1_1", "RollingBalls_Sci-fi_4_4" };
        public Transform itemPosition;
        private bool playerInRange = false;

        void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                TryToPlaceItems();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("wizard"))
            {
                Debug.Log("Wizard entered altar area.");
                playerInRange = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("wizard"))
            {
                Debug.Log("Wizard exited altar area.");
                playerInRange = false;
            }
        }

        void TryToPlaceItems()
        {
            Inventory inventory = GameObject.FindGameObjectWithTag("wizard").GetComponent<Inventory>();
            if (inventory != null)
            {
                bool allBallsPlaced = true;

                // Cek setiap bola apakah sudah dikumpulkan
                foreach (string requiredItem in requiredItems)
                {
                    if (inventory.items.Contains(requiredItem))
                    {
                        RemoveItemFromInventory(inventory, requiredItem);
                        Debug.Log(requiredItem + " placed on the altar.");
                    }
                    else
                    {
                        Debug.Log("Missing: " + requiredItem);
                        allBallsPlaced = false;
                    }
                }

                // Jika semua bola sudah ditempatkan, munculkan kunci
                if (allBallsPlaced)
                {
                    BallManager.Instance.CollectBall();
                }
                else
                {
                    Debug.Log("Not all required balls are placed.");
                }
            }
            else
            {
                Debug.Log("Inventory not found.");
            }
        }

        void RemoveItemFromInventory(Inventory inventory, string itemName)
        {
            int index = inventory.items.IndexOf(itemName);
            if (index >= 0)
            {
                if (index < inventory.itemObjects.Count)
                {
                    GameObject itemObject = inventory.itemObjects[index];
                    if (itemObject != null)
                    {
                        itemObject.SetActive(false);
                    }
                }
                inventory.itemObjects.RemoveAt(index);
                inventory.items.RemoveAt(index);
                Debug.Log(itemName + " removed from inventory.");
            }
        }
    }
}
