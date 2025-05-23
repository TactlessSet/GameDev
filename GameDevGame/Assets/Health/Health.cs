using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    private AttackAnimaticUI attackAnimaticUI;
    public string characterName = "New Character";
    public int maxHealth = 100;
    public int currentHealth;
    public Sprite combatSprite;

    public GameObject healthBar;  
    public TextMeshProUGUI healthText;


    public List<CharacterAction> actions = new List<CharacterAction>();

    public enum BuffType { DamageBoost, Invisibility, DefenseBoost, Immunity}
    public Dictionary<BuffType, int> activeBuffs = new Dictionary<BuffType, int>();

    void Awake()
    {
        currentHealth = maxHealth;
        if (attackAnimaticUI == null)
        {
            attackAnimaticUI = FindObjectOfType<AttackAnimaticUI>();
        }
        InitializeCharacterActions();
    }

    void InitializeCharacterActions()
    {
        switch (characterName)
        {
            case "Mage":
                actions.Add(new CharacterAction("Magic Ball", (user, target) =>
                {
                    if (target.CompareTag("Enemy"))
                    {
                        int baseDamage = 20;
                        int finalDamage = Mathf.FloorToInt(baseDamage * user.GetDamageMultiplier());
                        target.TakeDamage(finalDamage);
                        Debug.Log($"{user.characterName} casts Magic Ball at {target.characterName}!");
                        TriggerAnimatic(user, target);
                    }
                    else
                    {
                        Debug.Log("Magic Ball can only target enemies.");
                    }
                }) { requiresTarget = true });

                actions.Add(new CharacterAction("Power Infuse", (user, target) =>
                {
                    if (target.CompareTag("Ally"))
                    {
                        target.ApplyBuff(BuffType.DamageBoost, 2);
                        Debug.Log($"{user.characterName} infuses {target.characterName} with power!");
                        TriggerAnimatic(user, target);  
                    }
                    else
                    {
                        Debug.Log("Power Infuse can only target allies.");
                    }
                }) { requiresTarget = true });

                actions.Add(new CharacterAction("Cast Cloak", (user, target) =>
                {
                    target.ApplyBuff(BuffType.Invisibility, 2);
                    Debug.Log($"{target.characterName} becomes invisible!");
                    TriggerAnimatic(user, target);
                    TurnManager.Instance.OnPartyMemberActed();  
                    //"invisible"
                    SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
                    }
                    else
                    {
                        Debug.LogWarning($"{user.characterName} has no SpriteRenderer!");
                    }
                }) { requiresTarget = true });
                break;

            case "Knight":
                actions.Add(new CharacterAction("Shield", (user, target) =>
                {
                    target.ApplyBuff(BuffType.DefenseBoost, 2);
                    Debug.Log($"{user.characterName} shields their party member!");
                    TriggerAnimatic(user, target);
                    TurnManager.Instance.OnPartyMemberActed();
                }) { requiresTarget = true });

                actions.Add(new CharacterAction("Crossbow Shot", (user, target) =>
                {
                    int baseDamage = 20;
                    int finalDamage = Mathf.FloorToInt(baseDamage * user.GetDamageMultiplier());
                    target.TakeDamage(finalDamage);
                    Debug.Log($"{user.characterName} fires a crossbow shot at {target.characterName}!");
                    TriggerAnimatic(user, target);
                }) { requiresTarget = true });

                actions.Add(new CharacterAction("Crossbow FIRE", (user, target) =>
                {
                    target.ApplyBuff(BuffType.DamageBoost, 5);
                    user.StartCoroutine(user.ApplyFlamingDamageOverTime(target));
                    Debug.Log($"{user.characterName} fires a flaming arrow at {target.characterName}!");
                    TriggerAnimatic(user, target);
                }) { requiresTarget = true });
                break;

            case "Swordsman":
                actions.Add(new CharacterAction("Big Slash", (user, target) =>
                {
                    int baseDamage = 30;
                    int finalDamage = Mathf.FloorToInt(baseDamage * user.GetDamageMultiplier());
                    target.TakeDamage(finalDamage); // High damage value
                    Debug.Log($"{user.characterName} performs a Big Slash on {target.characterName}!");
                    TriggerAnimatic(user, target);
                }) { requiresTarget = true });

                actions.Add(new CharacterAction("Sword Block", (user, target) =>
                {
                    target.ApplyBuff(BuffType.DefenseBoost, 2);
                    Debug.Log($"{user.characterName} blocks damage for {target.characterName}!");
                    TriggerAnimatic(user, target);
                    TurnManager.Instance.OnPartyMemberActed();
                }) { requiresTarget = true });

                actions.Add(new CharacterAction("Rally the Troops", (user, target) =>
{
                foreach (var member in FindObjectsOfType<Health>())
                {
                    if (member != user && member.CompareTag("Ally"))
                    {
                        member.ApplyBuff(BuffType.DamageBoost, 1);
                    }
                }
                Debug.Log($"{user.characterName} rallies the troops, doubling damage for the next round!");
                TriggerAnimatic(user, null);
                TurnManager.Instance.OnPartyMemberActed();
                }) { requiresTarget = true });
                break;

            case "Healer":
                actions.Add(new CharacterAction("Immunity", (user, target) =>
                {
                    foreach (var ally in FindObjectsOfType<Health>())
                    {
                        if (ally.CompareTag("Ally"))
                        {
                            ally.ApplyBuff(BuffType.Immunity, 2);//change buff type later
                            Debug.Log($"{user.characterName} makes {ally.characterName} immune to debuffs for 2 rounds!");
                        }
                    }
                    TriggerAnimatic(user, target);
                }) { requiresTarget = true });

                actions.Add(new CharacterAction("Heal", (user, target) =>
                {
                    //heal ally 20-30%
                    if (target.CompareTag("Ally"))
                    {
                        int healAmount = Mathf.FloorToInt(target.maxHealth * Random.Range(0.3f, 0.5f));
                        target.Heal(healAmount);
                        Debug.Log($"{user.characterName} heals {target.characterName} for {healAmount} HP!");
                        //TriggerAnimatic(user, target);

                    }
                    else
                    {
                        Debug.Log("Heal can only target allies.");
                    }
                }) { requiresTarget = true });

                actions.Add(new CharacterAction("Light Attack", (user, target) =>
                {
                    //weak attack (cus everyone deserves something)
                    int baseDamage = 10;
                    int finalDamage = Mathf.FloorToInt(baseDamage * user.GetDamageMultiplier());
                    target.TakeDamage(finalDamage);
                    Debug.Log($"{user.characterName} hits {target.characterName} with a Light Attack for {finalDamage} damage!");
                    TriggerAnimatic(user, target);
                }) { requiresTarget = true });
                break;
                
            case "Rogue":
                actions.Add(new CharacterAction("Knife Throw", (user, target) =>
                {
                    int baseDamage = 15;
                    int finalDamage = Mathf.FloorToInt(baseDamage * user.GetDamageMultiplier());
                    target.TakeDamage(finalDamage);
                    Debug.Log($"{user.characterName} throws a knife at {target.characterName} for {finalDamage} damage!");
                    TriggerAnimatic(user, target);
                }) { requiresTarget = true });

                actions.Add(new CharacterAction("Cloak", (user, target) =>
                {
                    //cloak for 2 turns
                    target.ApplyBuff(BuffType.Invisibility, 2);
                    Debug.Log($"{user.characterName} cloaks their party member, making them untargetable for 2 turns!");
                    TriggerAnimatic(user, null);
                }) { requiresTarget = true });

                actions.Add(new CharacterAction("Heal Self", (user, target) =>
                {
                    int healAmount = Mathf.FloorToInt(user.maxHealth * 0.1f); //10% of their health
                    user.Heal(healAmount);
                    Debug.Log($"{user.characterName} heals themself for {healAmount} HP!");
                    //TriggerAnimatic(user, null);
                }) { requiresTarget = false });
                break;


        }
    }



    private void TriggerAnimatic(Health attacker, Health victim)
    {
        Sprite attackerSprite = attacker.combatSprite;
        Sprite victimSprite = victim != null ? victim.combatSprite : null;
        Debug.Log($"Triggering animatic with attacker: {attackerSprite}, victim: {victimSprite}");
        if (attackAnimaticUI != null)
        {
            attackAnimaticUI.PlayAnimatic(attackerSprite, victimSprite);
        }
    }

    public void TakeDamage(int damage)
    {
        if (activeBuffs.ContainsKey(BuffType.DefenseBoost))
        {
            damage = Mathf.FloorToInt(damage * 0.5f); //50% damage reduction
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log(characterName + " took " + damage + " damage. Remaining HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private IEnumerator ApplyFlamingDamageOverTime(Health target)
    {
        int ticks = 5;
        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(1f);
            target.TakeDamage(2); 
            Debug.Log($"{target.characterName} is burned by the flaming arrow! 2 damage.");
        }
    }
    
    public bool IsImmuneTo(BuffType buff)
    {
        return activeBuffs.ContainsKey(BuffType.Immunity) && buff != BuffType.Immunity;
    }


    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log(characterName + " healed " + amount + " HP. Current HP: " + currentHealth);
        TriggerAnimatic(this, null);
    }

    private void Die()
    {
        Debug.Log(characterName + " has died!");

        if (CompareTag("Enemy"))
        {
            LevelManager.Instance.CheckIfEnemiesDefeated();
        }
       

        TurnManager tm = FindObjectOfType<TurnManager>();
        if (tm != null)
        {
            tm.RemoveFromTurnOrder(this);
        }
        healthBar.SetActive(false);
        healthText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ApplyBuff(BuffType type, int duration)
    {
        if (IsImmuneTo(type))
        {
            Debug.Log($"{characterName} is immune to {type} and resisted the effect.");
            return;
        }
        if (activeBuffs.ContainsKey(type))
            activeBuffs[type] = duration;
        else
            activeBuffs.Add(type, duration);
        
        if (type == BuffType.DefenseBoost)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = Color.cyan;
        }
    }

    public void TickBuffs()
    {
        var keys = new List<BuffType>(activeBuffs.Keys);
        foreach (var key in keys)
        {
            activeBuffs[key]--;

            if (activeBuffs[key] <= 0)
            {
                activeBuffs.Remove(key);

                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (key == BuffType.Invisibility)
                    {
                        sr.color = new Color(1f, 1f, 1f, 1f); // Fully visible
                    }
                    else if (key == BuffType.DefenseBoost)
                    {
                        sr.color = Color.white; // Reset color
                    }
                }
            }
        }
    }


    public float GetDamageMultiplier()
    {
        return activeBuffs.ContainsKey(BuffType.DamageBoost) ? 2f : 1f;
    }

}
