using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Health> partyMembers;
    public List<Health> enemies;

    private int currentTurnIndex = 0;
    private bool isPlayerTurn = true;
    private Health currentPartyMember;

    void Start()
    {
        BeginPlayerTurn();
    }

    void AdvanceToNextPartyMember()
    {
        if (currentTurnIndex >= partyMembers.Count)
        {
            EndPlayerTurn();
            return;
        }

        currentPartyMember = partyMembers[currentTurnIndex];

        // Skip dead members
        if (currentPartyMember.currentHealth <= 0)
        {
            currentTurnIndex++;
            AdvanceToNextPartyMember(); //skip dead
            return;
        }

        Debug.Log($"It's {currentPartyMember.name}'s turn!");
    }

    void BeginPlayerTurn()
    {
        isPlayerTurn = true;
        currentTurnIndex = 0;
        AdvanceToNextPartyMember();
        // Player turn
    }

    public void OnPartyMemberActed()
    {
        currentTurnIndex++;
        AdvanceToNextPartyMember();
    }

    public void EndPlayerTurn()
    {
        BeginEnemyTurn();
    }

    void BeginEnemyTurn()
    {
        isPlayerTurn = false;
        currentTurnIndex = 0;
        Debug.Log("Enemy turn begins");
        StartCoroutine(EnemyTurnSequence());
    }

    System.Collections.IEnumerator EnemyTurnSequence()
    {
        while (currentTurnIndex < enemies.Count)
        {
            Health enemy = enemies[currentTurnIndex];
            if (enemy != null && enemy.currentHealth > 0)
            {
                //enemy move
                Health target = partyMembers[Random.Range(0, partyMembers.Count)];
                if (target != null && target.currentHealth > 0)
                {
                    Debug.Log($"{enemy.name} attacks {target.name}");
                    target.TakeDamage(10); 
                    yield return new WaitForSeconds(1f);
                }
            }

            currentTurnIndex++;
        }

        yield return new WaitForSeconds(1f);
        BeginPlayerTurn();
    }
}