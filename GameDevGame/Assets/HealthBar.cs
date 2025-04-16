using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health stats;
    public Slider slider;

    void Start()
    {
        if (stats != null)
        {
            slider.maxValue = stats.maxHealth;
            slider.value = stats.currentHealth;
        }
    }

    void Update()
    {
        if (stats != null)
        {
            slider.value = stats.currentHealth;
        }
    }
}