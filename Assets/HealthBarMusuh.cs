using UnityEngine;
using UnityEngine.UI;

public class HealthBarMusuh : MonoBehaviour
{
    public Slider slider; // Slider untuk health bar
    public Image fillImage; // Image untuk bagian fill dari slider

    public Color normalColor = Color.green; // Hijau untuk kondisi normal
    public Color freezeColor = new Color(0.5f, 0.5f, 1f); // Biru muda untuk freeze

    private bool isFrozen = false; // Status freeze

    void Start()
    {
        // Cari komponen otomatis jika tidak diatur di Inspector
        if (slider == null)
        {
            slider = GetComponent<Slider>();
            if (slider == null)
            {
                Debug.LogError("Slider component is missing in HealthBarMusuh!");
            }
        }

        if (fillImage == null && slider != null)
        {
            fillImage = slider.fillRect.GetComponent<Image>();
            if (fillImage == null)
            {
                Debug.LogError("Fill Image is missing in HealthBarMusuh!");
            }
        }

        // Set warna awal
        UpdateColor();
    }

    public void SetMaxHealth(int maxHealth)
    {
        if (slider == null || fillImage == null)
        {
            Debug.LogError("Slider or Fill Image is not assigned in HealthBarMusuh!");
            return;
        }

        slider.maxValue = maxHealth; // Set nilai maksimum
        slider.value = maxHealth;    // Atur ke nilai maksimum
        UpdateColor();               // Perbarui warna
    }

    public void SetHealth(int health)
    {
        if (slider == null || fillImage == null)
        {
            Debug.LogError("Slider or Fill Image is not assigned in HealthBarMusuh!");
            return;
        }

        slider.value = Mathf.Clamp(health, 0, slider.maxValue); // Batasan nilai health
        UpdateColor();         // Perbarui warna
    }

    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
        UpdateColor(); // Perbarui warna saat status freeze berubah
    }

    public void UpdateColor()
    {
        if (slider == null || fillImage == null) return;

        fillImage.color = isFrozen ? freezeColor : normalColor; // Ubah warna berdasarkan status freeze
    }
}