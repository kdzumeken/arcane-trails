using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.UI; // Import UI namespace for RawImage

public class NarrationSceneManager : MonoBehaviour
{
    public CanvasGroup canvasGroup; // Canvas Group untuk efek fade
    public TextMeshProUGUI narrationText; // TextMeshProUGUI untuk narasi
    public string[] narrationLines; // Array narasi
    public float fadeDuration = 2f; // Durasi fade
    public float textDisplayDuration = 3f; // Waktu tampilan tiap narasi
    public string mainMenuSceneName = "MainMenu"; // Nama scene Main Menu
    public RawImage backgroundImage; // RawImage untuk background
    public AudioSource audioSource; // AudioSource untuk musik/narasi

    private int currentLineIndex = 0;

    void Start()
    {
        narrationText.alpha = 0; // Awal teks transparan
        StartCoroutine(PlayNarration());
    }

    IEnumerator PlayNarration()
    {
        while (currentLineIndex < narrationLines.Length)
        {
            // Set narasi sebelum fade in
            narrationText.text = narrationLines[currentLineIndex];
            currentLineIndex++;

            // Fade in teks
            yield return StartCoroutine(FadeText(0, 1, fadeDuration));

            // Tunggu selama durasi tampilan teks
            yield return new WaitForSeconds(textDisplayDuration);

            // Fade out teks
            yield return StartCoroutine(FadeText(1, 0, fadeDuration));
        }

        // Mulai fade out audio sebelum pindah scene
        yield return StartCoroutine(FadeAudioOut(audioSource, fadeDuration));

        // Pindah ke Main Menu
        SceneManager.LoadScene(mainMenuSceneName);
    }

    IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            narrationText.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }
        narrationText.alpha = endAlpha;
    }

    IEnumerator FadeAudioOut(AudioSource audioSource, float duration)
    {
        if (audioSource != null)
        {
            float startVolume = audioSource.volume;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
                yield return null;
            }

            audioSource.volume = 0;
        }
    }
}
