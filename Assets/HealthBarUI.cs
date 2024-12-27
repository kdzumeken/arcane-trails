using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider slider; // Slider untuk health bar
    public Image fillImage; // Image untuk bagian fill dari slider

    // Warna berdasarkan tingkat kesehatan
    public Color highHealthColor = Color.green; // Hijau untuk kesehatan tinggi
    public Color midHealthColor = Color.yellow; // Kuning untuk kesehatan sedang
    public Color lowHealthColor = new Color(1f, 0.65f, 0f); // Oranye untuk kesehatan rendah
    public Color criticalHealthColor = Color.red; // Merah untuk kesehatan kritis

    public void SetMaxHealth(int maxHealth)
    {
        if (slider == null || fillImage == null)
        {
            Debug.LogError("Slider or Fill Image is not assigned in HealthBarUI!");
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
            Debug.LogError("Slider or Fill Image is not assigned in HealthBarUI!");
            return;
        }

        slider.value = health; // Update nilai health
        UpdateColor();         // Perbarui warna
    }

    private void UpdateColor()
    {
        if (slider == null || fillImage == null) return;

        float healthPercentage = slider.value / slider.maxValue; // Hitung persentase kesehatan

        // Tentukan warna berdasarkan persentase kesehatan
        if (healthPercentage > 0.7f)
        {
            fillImage.color = highHealthColor; // Hijau
        }
        else if (healthPercentage > 0.4f)
        {
            fillImage.color = midHealthColor; // Kuning
        }
        else if (healthPercentage > 0.2f)
        {
            fillImage.color = lowHealthColor; // Oranye
        }
        else
        {
            fillImage.color = criticalHealthColor; // Merah
        }
    }
}
