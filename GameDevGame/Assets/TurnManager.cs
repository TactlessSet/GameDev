using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Health> partyMembers;
    public List<Health> enemies;
    public static TurnManager Instance;
    private List<Health> turnOrder = new List<Health>();
    private int turnIndex = 0;


    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        turnOrder.Clear(); // more of a debug thing than anything; not necessaryr
        //combine entities
        turnOrder.AddRange(partyMembers);
        turnOrder.AddRange(enemies);
        foreach (var unit in turnOrder)
        {
            //Debug.Log($"Added to turnOrder: {unit.name}");
        }
        ContinueTurnCycle();
    }

    void ContinueTurnCycle()
    {
        if (turnOrder.Count == 0)
        {
            Debug.LogError("No combatants found.");
            return;
        }

        if (turnIndex >= turnOrder.Count)
        {
            turnIndex = 0;
        }

        Health current = turnOrder[turnIndex];

        // Skip dead units
        if (current.currentHealth <= 0)
        {
            turnIndex++;
            ContinueTurnCycle();
            return;
        }

        if (partyMembers.Contains(current))
        {
            Debug.Log($"It's {current.name}'s turn (Player)!");
            FindObjectOfType<PlayerCombatController>().EnableActionPanel();
        }
        else if (enemies.Contains(current))
        {
            Debug.Log($"It's {current.name}'s turn (Enemy)!");
            StartCoroutine(EnemyAct(current));
        }
    }

    public Health GetCurrentPartyMember()
    {
        if (turnIndex < turnOrder.Count && partyMembers.Contains(turnOrder[turnIndex]))
        {
            return turnOrder[turnIndex];
        }

        return null;
    }

    public void OnPartyMemberActed()
    {
        var lastActor = turnOrder[turnIndex];
        lastActor.GetComponent<Health>().TickBuffs();

        turnIndex++;
        ContinueTurnCycle();
    }

    IEnumerator EnemyAct(Health enemy)
    {
        yield return new WaitForSeconds(0.5f);

        List<Health> validTargets = partyMembers.FindAll(p => p.currentHealth > 0);
        if (validTargets.Count > 0)
        {
            Health target = validTargets[Random.Range(0, validTargets.Count)];
            Debug.Log($"{enemy.name} attacks {target.name}");
            target.TakeDamage(10);
        }

        yield return new WaitForSeconds(1f);
        turnIndex++;
        ContinueTurnCycle();
    }
}
