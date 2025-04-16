using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public Health stats;
    public TextMeshProUGUI healthText;

    void Update()
    {
        if (stats != null && healthText != null)
        {
            healthText.text = stats.currentHealth + " / " + stats.maxHealth;
        }
    }
}
