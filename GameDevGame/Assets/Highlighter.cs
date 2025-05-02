using UnityEngine;

public class Highlighter : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;
    public Color enemyHighlightColor = Color.red;
    public Color allyHighlightColor = Color.green;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError($"No SpriteRenderer found on {gameObject.name}");
        }
        originalColor = sr?.color ?? Color.white; // Default to white if no color is set
    }

    private void OnMouseEnter()
    {
        if (PlayerCombatController.Instance != null && PlayerCombatController.Instance.IsTargeting)
        {
            // Check if the target is an enemy or ally and highlight accordingly
            if (CompareTag("Enemy"))
            {
                sr.color = enemyHighlightColor;
            }
            else if (CompareTag("Ally"))
            {
                sr.color = allyHighlightColor;
            }
        }
    }

    private void OnMouseExit()
    {
        sr.color = originalColor;
    }

    private void OnMouseDown()
    {
        if (PlayerCombatController.Instance != null && PlayerCombatController.Instance.IsTargeting)
        {
            var health = GetComponent<Health>();
            if (health != null)
            {
                // Ensure the action panel is active and the target selection is valid
                if (PlayerCombatController.Instance.IsTargeting)
                {
                    Debug.Log($"Target selected: {health.characterName}");
                    PlayerCombatController.Instance.ReceiveTargetSelection(health);
                }
            }
            else
            {
                Debug.LogWarning("No Health component found on the clicked target.");
            }
        }
    }
}
