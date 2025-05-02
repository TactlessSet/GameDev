using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerCombatController : MonoBehaviour
{
    public static PlayerCombatController Instance { get; private set; }

    public TurnManager turnManager;
    public GameObject actionPanel;
    public AttackAnimaticUI animaticUI;

    public Sprite exampleAttackerSprite;
    public Sprite exampleVictimSprite;

    public bool IsTargeting => isTargeting;
    private bool isTargeting = false;
    private bool waitingForTarget = false;

    private CharacterAction currentAction;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        actionPanel.SetActive(false);
    }

    void Update()
    {
        if (isTargeting && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null)
            {
                Health target = hit.GetComponent<Health>();
                if (target != null)
                {
                    // Validate target selection based on action
                    if (IsValidTarget(target))
                    {
                        ReceiveTargetSelection(target);
                    }
                }
            }
        }
    }

    // Validate if the target can be selected
    private bool IsValidTarget(Health target)
    {
        if (target.CompareTag("Enemy") && currentAction != null && currentAction.requiresTarget)
        {
            return true;  // Enemy can be selected if the action targets enemies
        }
        else if (target.CompareTag("Ally") && currentAction != null && currentAction.requiresTarget)
        {
            return true;  // Ally can be selected if the action targets allies
        }
        return false;  // Invalid target if not an ally or enemy as per the action requirement
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

        // Reset targeting UI state
        isTargeting = false;
        actionPanel.SetActive(false);
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
        Debug.Log("EnableActionPanel() was called");

        actionPanel.SetActive(true);
        var currentChar = turnManager.GetCurrentPartyMember();
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

        Health user = turnManager.GetCurrentPartyMember();

        if (action.requiresTarget)
        {
            currentAction = action;
            waitingForTarget = true;
            isTargeting = true;
            Debug.Log("Awaiting target selection...");
        }
        else
        {
            // Directly execute if no target is needed
            action.ExecuteAction(user, user); 
            OnActionCompleted();
            actionPanel.SetActive(false);
        }
    }

    public void ReceiveTargetSelection(Health target)
    {
        if (waitingForTarget && currentAction != null)
        {
            Health user = turnManager.GetCurrentPartyMember();

            Debug.Log($"Target selected: {target.characterName}");
            currentAction.ExecuteAction(user, target);

            waitingForTarget = false;
            isTargeting = false;
            actionPanel.SetActive(false);

            OnActionCompleted();
        }
    }

    private void OnActionCompleted()
    {
        turnManager.OnPartyMemberActed();
    }
}
