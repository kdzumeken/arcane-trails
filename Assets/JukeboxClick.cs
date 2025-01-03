using System.Collections;
using UnityEngine;

public class JukeboxClick : MonoBehaviour
{
    public AudioClip[] soundClips; // Drag your 4 MP3 sound clips here in the Inspector.
    private AudioSource audioSource;
    public Outline outline; // Reference to the Outline component

    private void Start()
    {
        // Add an AudioSource component to the GameObject if it doesn't have one already.
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Ensure the outline is initially disabled
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        // Play each sound clip sequentially with 0.5-second intervals.
        if (soundClips != null && soundClips.Length > 0 && !audioSource.isPlaying)
        {
            StartCoroutine(PlayMultipleSounds(1f));
        }
    }

    private IEnumerator PlayMultipleSounds(float interval)
    {
        foreach (var clip in soundClips)
        {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(interval);
            audioSource.Stop();
        }
    }

    private void OnMouseEnter()
    {
        // Enable the outline when the mouse enters the object
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        // Disable the outline when the mouse exits the object
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}
