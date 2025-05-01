using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerCombatController : MonoBehaviour
{
    public static PlayerCombatController Instance;
    public TurnManager turnManager;
    public GameObject actionPanel;
    public AttackAnimaticUI animaticUI; 

    public Sprite exampleAttackerSprite;
    public Sprite exampleVictimSprite;
    public bool IsTargeting => isTargeting;

    private bool isTargeting = false;

    void Start()
    {
        Instance = this;
        actionPanel.SetActive(false); //WORK PLEASEEEEE
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
        Sprite attackerSprite = attacker.combatSprite;
        Sprite victimSprite = enemy.combatSprite;

        animaticUI.PlayAnimatic(attackerSprite, victimSprite);
        int damage = 10; 
        Debug.Log($"{attacker.name} attacks {enemy.name} for {damage} damage!");
        StartCoroutine(DelayedAttack(enemy, damage));
        //enemy.TakeDamage(damage);

        isTargeting = false;
        actionPanel.SetActive(false);
       // turnManager.OnPartyMemberActed();
    }

    IEnumerator DelayedAttack(Health enemy, int damage)
    {
        yield return new WaitForSeconds(1.5f); 

        enemy.TakeDamage(damage);
        actionPanel.SetActive(false);
        turnManager.OnPartyMemberActed();
    }

    public void EnableActionPanel()
    {
        var currentChar = TurnManager.Instance.GetCurrentPartyMember();
        if (currentChar == null)
        {
            Debug.LogError("Current character is null!");
            return;
        }

        var health = currentChar.GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("Health component missing from current character!");
            return;
        }

        // Check if actionPanel is properly assigned in the Inspector
        if (actionPanel == null)
        {
            Debug.LogError("ActionPanel GameObject is not assigned in the Inspector!");
            return;
        }

        // Get ActionPanelUI component from the actionPanel GameObject
        ActionPanelUI actionUI = actionPanel.GetComponent<ActionPanelUI>();
        if (actionUI != null)
        {
            actionUI.ShowActions(health.actions);
        }
        else
        {
            Debug.LogError("ActionPanelUI component not found on actionPanel GameObject.");
        }
    }

    public void PrepareAction(CharacterAction action)
    {
        Debug.Log($"Preparing action: {action.actionName}");

        Health user = turnManager.GetCurrentPartyMember(); // the character doing the action
        Health target = user; // default to targeting self

        action.ExecuteAction(user, target);

        actionPanel.SetActive(false);
        turnManager.OnPartyMemberActed();
    }
}