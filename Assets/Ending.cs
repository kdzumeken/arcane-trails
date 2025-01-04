using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    [SerializeField] private RawImage whiteImage; // Reference to the white RawImage
    [SerializeField] private float fadeDuration = 2f; // Duration of the fade in seconds

    private void Start()
    {
        // Hide the white image at the start
        whiteImage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Unlock the cursor
            Wizard playerWizard = other.GetComponent<Wizard>();
            if (playerWizard != null)
            {
                playerWizard.EnablePointerMode();
            }

            StartCoroutine(FadeInAndLoadScene());
        }
    }

    private IEnumerator FadeInAndLoadScene()
    {
        float elapsedTime = 0f;
        Color color = whiteImage.color;
        color.a = 0f;
        whiteImage.color = color;
        whiteImage.gameObject.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            whiteImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene("Ending");
    }
}

