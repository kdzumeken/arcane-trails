using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahScene : MonoBehaviour
{
    public string sceneName; // Nama scene yang akan dimasuki

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method ini akan dipanggil ketika collider dengan isTrigger diaktifkan
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Pastikan hanya player yang bisa memicu pergantian scene
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}

