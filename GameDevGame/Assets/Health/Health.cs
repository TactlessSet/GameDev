using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public string characterName = "New Character";
    public int maxHealth = 100;
    public int currentHealth;
    public Sprite combatSprite;

    public List<CharacterAction> actions = new List<CharacterAction>();

    public enum BuffType { DamageBoost, Invisibility }

    public Dictionary<BuffType, int> activeBuffs = new Dictionary<BuffType, int>();

    void Start()
    {
        currentHealth = maxHealth;
        InitializeCharacterActions();
    }

    void InitializeCharacterActions()
    {
        switch (characterName)
        {
            case "Mage":
                actions.Add(new CharacterAction("Magic Ball", (user, target) =>
                {
                    target.TakeDamage(15);
                    Debug.Log($"{user.characterName} casts Magic Ball at {target.characterName}!");
                }));

                actions.Add(new CharacterAction("Power Infuse", (user, target) =>
                {
                    target.ApplyBuff(BuffType.DamageBoost, 2);
                    Debug.Log($"{user.characterName} infuses {target.characterName} with power!");
                }));

                actions.Add(new CharacterAction("Cloak Self", (user, target) => 
                {
                    user.ApplyBuff(BuffType.Invisibility, 2);
                    Debug.Log($"{user.characterName} becomes invisible!");
                }));
                break;

            
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log(characterName + " took " + damage + " damage. Remaining HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log(characterName + " healed " + amount + " HP. Current HP: " + currentHealth);
    }

    private void Die()
    {
        Debug.Log(characterName + " has died!");
        // Add death handling logic here later
    }

    public void ApplyBuff(BuffType type, int duration)
    {
        if (activeBuffs.ContainsKey(type))
            activeBuffs[type] = duration;
        else
            activeBuffs.Add(type, duration);
    }

    public void TickBuffs()
    {
        var keys = new List<BuffType>(activeBuffs.Keys);
        foreach (var key in keys)
        {
            activeBuffs[key]--;
            if (activeBuffs[key] <= 0)
                activeBuffs.Remove(key);
        }
    }
}
