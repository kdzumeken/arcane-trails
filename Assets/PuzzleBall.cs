using System.Collections;
using UnityEngine;

namespace SunTemple
{
    public class PuzzleBall : MonoBehaviour
    {
        public AltarBall altar;
        public GameObject key;
        public Transform keyPosition;

        void Start()
        {
            key?.SetActive(false);
        }

        void Update()
        {
            if (altar.AreAllItemsPlaced())
            {
                DropKey();
            }
        }

        void DropKey()
        {
            if (key != null && keyPosition != null && !key.activeSelf)
            {
                key.SetActive(true);
                key.transform.position = keyPosition.position;

                Rigidbody rb = key.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Destroy(rb);
                }

                Debug.Log("The key has appeared.");
            }
        }
    }
}
