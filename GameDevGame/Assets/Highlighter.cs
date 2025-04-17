using UnityEngine;

public class SpriteRendererHighlighter : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;
    public Color highlightColor = Color.red;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    private void OnMouseEnter()
    {
        if (PlayerCombatController.Instance != null && PlayerCombatController.Instance.IsTargeting)
        {
            sr.color = highlightColor;
        }
    }

    private void OnMouseExit()
    {
        sr.color = originalColor;
    }
}
