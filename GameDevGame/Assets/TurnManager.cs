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
        turnOrder.Clear(); //more of a debug thing than anything; not necessary
        
        turnOrder.AddRange(partyMembers);
        turnOrder.AddRange(enemies);
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

        //skip dead
        if (current.currentHealth <= 0)
        {
            turnIndex++;
            ContinueTurnCycle();
            return;
        }

        if (partyMembers.Contains(current))
        {
            Debug.Log($"It's {current.characterName}'s turn (Player)!");
            FindObjectOfType<PlayerCombatController>().EnableActionPanel();
        }

        else if (enemies.Contains(current))
        {
            Debug.Log($"It's {current.characterName}'s turn (Enemy)!");
            StartCoroutine(EnemyAct(current));
        }
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

    void NextTurn()
    {
        if (turnOrder.Count == 0)
        {
            Debug.Log("All characters have died!");
            return;
        }

        turnIndex = (turnIndex + 1) % turnOrder.Count;
        ContinueTurnCycle();
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
        lastActor.GetComponent<Health>().TickBuffs(); // Tick buffs after action

        turnIndex = (turnIndex + 1) % turnOrder.Count; // Ensure turnIndex wraps around
        ContinueTurnCycle(); // Continue to the next turn
    }


    //enemy shtuffs
    IEnumerator EnemyAct(Health enemy)
    {
        yield return new WaitForSeconds(0.5f); 

        List<Health> validTargets = partyMembers.FindAll(p =>
        p.currentHealth > 0 &&
        (!p.activeBuffs.ContainsKey(Health.BuffType.Invisibility))
        );
        if (validTargets.Count > 0)
        {
            Health target = validTargets[Random.Range(0, validTargets.Count)];
            Debug.Log($"{enemy.characterName} attacks {target.characterName}");

            target.TakeDamage(30);
            //TriggerAnimatic(enemy, target);

            yield return new WaitForSeconds(1f);
        }

        turnIndex++;
        ContinueTurnCycle();
    }

    /*private void TriggerAnimatic(Health attacker, Health victim)
    {
        Sprite attackerSprite = attacker.combatSprite;
        Sprite victimSprite = victim != null ? victim.combatSprite : null;
        if (PlayerCombatController.Instance.animaticUI != null)
        {
            PlayerCombatController.Instance.animaticUI.PlayAnimatic(attackerSprite, victimSprite);
        }
    } */
}
