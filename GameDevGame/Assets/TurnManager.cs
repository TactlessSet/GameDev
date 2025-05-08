using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Health> partyMembers;
    public List<Health> enemies;
    public static TurnManager Instance;
    public GameObject turnIndicator;
    private List<Health> turnOrder = new List<Health>();
    private int turnIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        turnOrder.Clear();
        turnOrder.AddRange(partyMembers);
        turnOrder.AddRange(enemies);
        ContinueTurnCycle();
    }

    void ContinueTurnCycle()
    {
        if (turnOrder.Count == 0)
        {
            Debug.LogError("No combatants in turn order.");
            return;
        }

        if (turnIndex >= turnOrder.Count)
        {
            turnIndex = 0;
        }

        Health current = turnOrder[turnIndex];

        // Skip dead characters
        if (current == null || current.currentHealth <= 0)
        {
            Debug.Log($"{(current != null ? current.characterName : "NULL")} is dead or null. Skipping.");
            turnIndex++;
            ContinueTurnCycle();
            return;
        }

        if (partyMembers.Contains(current))
        {
            Debug.Log($"It's {current.characterName}'s turn (Player)!");
            FindObjectOfType<PlayerCombatController>()?.EnableActionPanel();

            turnIndicator.SetActive(true);
            Vector3 offset = new Vector3(0, -1f, 0);
            turnIndicator.transform.position = current.transform.position + offset;

        }
        else if (enemies.Contains(current))
        {
            Debug.Log($"It's {current.characterName}'s turn (Enemy)!");
            if (turnIndicator != null) turnIndicator.SetActive(false);
            StartCoroutine(EnemyAct(current));
        }
    }

    public void OnPartyMemberActed()
    {
        var lastActor = turnOrder[turnIndex];
        lastActor?.TickBuffs();
        NextTurn();
    }

    void NextTurn()
    {
        if (turnOrder.Count == 0)
        {
            Debug.Log("Combat has ended or all characters are dead.");
            return;
        }

        turnIndex = (turnIndex + 1) % turnOrder.Count;
        ContinueTurnCycle();
    }

    public void RemoveFromTurnOrder(Health deadCharacter)
    {
        if (turnOrder.Contains(deadCharacter))
        {
            int index = turnOrder.IndexOf(deadCharacter);
            turnOrder.Remove(deadCharacter);

            if (index <= turnIndex && turnIndex > 0)
            {
                turnIndex--;
            }
        }
    }

    public Health GetCurrentPartyMember()
    {
        if (turnIndex < turnOrder.Count)
        {
            Health current = turnOrder[turnIndex];
            if (partyMembers.Contains(current))
            {
                return current;
            }
        }

        return null;
    }

    IEnumerator EnemyAct(Health enemy)
    {
        yield return new WaitForSeconds(0.5f);

        List<Health> validTargets = partyMembers.FindAll(p =>
            p.currentHealth > 0 &&
            !p.activeBuffs.ContainsKey(Health.BuffType.Invisibility)
        );

        if (validTargets.Count > 0)
        {
            Health target = validTargets[Random.Range(0, validTargets.Count)];
            Debug.Log($"{enemy.characterName} attacks {target.characterName}");
            target.TakeDamage(30);
            yield return new WaitForSeconds(1f);
        }
        else
        {
            Debug.Log($"{enemy.characterName} has no visible targets.");
        }

        enemy.TickBuffs();
        NextTurn();
    }
}
