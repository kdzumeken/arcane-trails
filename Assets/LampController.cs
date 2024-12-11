using UnityEngine;

public class LampController : MonoBehaviour
{
    public Light lampLight; // Lampu yang akan menyala/mati
    public Color onColor; // Warna saat lampu menyala
    public Color offColor = Color.black; // Warna saat lampu mati
    private bool isOn = false; // Status lampu (Menyala/Mati)
    private PuzzleManager puzzleManager; // Reference to the PuzzleManager

    private void Start()
    {
        // Find the PuzzleManager in the scene
        puzzleManager = FindObjectOfType<PuzzleManager>();
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
        if (puzzleManager.IsPuzzleSolved()) return; // Jika puzzle sudah terpecahkan, tidak bisa menekan lampu

        // Find the index of this lamp in the PuzzleManager's lamps array
        int lampIndex = System.Array.IndexOf(puzzleManager.lamps, this);
        if (lampIndex != -1)
        {
            puzzleManager.LampPressed(lampIndex);
        }
    }
}
