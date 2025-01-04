using UnityEngine;

public class LampController : MonoBehaviour
{
    public Light lampLight; // Lampu yang akan menyala/mati
    public Color onColor; // Warna saat lampu menyala
    public Color offColor = Color.black; // Warna saat lampu mati
    private bool isOn = false; // Status lampu (Menyala/Mati)
    public PuzzleManager puzzleManager; // Reference to the PuzzleManager, assign in Inspector
    private Outline outline; // Reference to the Outline component

    private void Start()
    {
        // Ensure the PuzzleManager is assigned
        if (puzzleManager == null)
        {
            Debug.LogError("PuzzleManager is not assigned to the LampController on " + gameObject.name);
        }

        // Get the Outline component
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogError("Outline component is not assigned to the LampController on " + gameObject.name);
        }
        else
        {
            outline.enabled = false; // Disable outline initially
        }
    }

    // Fungsi untuk menyalakan/mematikan lampu
    public void ToggleLamp()
    {
        isOn = !isOn;
        lampLight.color = isOn ? onColor : offColor; // Mengubah warna lampu
        lampLight.enabled = isOn; // Mengaktifkan/menonaktifkan komponen Light
    }

    // Fungsi untuk memeriksa status lampu
    public bool IsOn()
    {
        return isOn;
    }

    private void OnMouseDown()
    {
        if (puzzleManager == null || puzzleManager.IsPuzzleSolved()) return; // Jika puzzle sudah terpecahkan, tidak bisa menekan lampu

        // Find the index of this lamp in the PuzzleManager's lamps array
        int lampIndex = System.Array.IndexOf(puzzleManager.lamps, this);
        if (lampIndex != -1)
        {
            puzzleManager.LampPressed(lampIndex);
        }
    }

    private void OnMouseEnter()
    {
        if (outline != null)
        {
            outline.enabled = true; // Enable outline when mouse enters
        }
    }

    private void OnMouseExit()
    {
        if (outline != null)
        {
            outline.enabled = false; // Disable outline when mouse exits
        }
    }
}
