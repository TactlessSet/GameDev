using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatController : MonoBehaviour
{
    public static PlayerCombatController Instance;
    public TurnManager turnManager;
    public GameObject actionPanel;
    public bool IsTargeting => isTargeting;

    private bool isTargeting = false;

    void Start()
    {
        Instance = this;
        actionPanel.SetActive(false); //hide by default
    }

    void Update()
    {
        if (isTargeting && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null && hit.CompareTag("Enemy"))
            {
                Health enemyStats = hit.GetComponent<Health>();
                if (enemyStats != null)
                {
                    AttackEnemy(enemyStats);
                }
            }
        }
    }

    public void OnAttackButtonClicked()
    {
        isTargeting = true;
        Debug.Log("Choose an enemy to attack!");
    }

    void AttackEnemy(Health enemy)
    {
        Health attacker = turnManager.GetCurrentPartyMember();

        int damage = 10; 
        Debug.Log($"{attacker.name} attacks {enemy.name} for {damage} damage!");
        enemy.TakeDamage(damage);

        isTargeting = false;
        actionPanel.SetActive(false);
        turnManager.OnPartyMemberActed();
    }

    public void EnableActionPanel()
    {
        actionPanel.SetActive(true);
    }
}