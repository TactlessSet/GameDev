using System;
using UnityEngine;

[System.Serializable]
public class CharacterAction
{
    public string actionName;
    
    public Action<Health, Health> actionEffect;
    public bool requiresTarget;

    public CharacterAction(string name, Action<Health, Health> effect, bool targetRequired = true)
    {
        actionName = name;
        actionEffect = effect ?? throw new ArgumentNullException(nameof(effect), "Action effect cannot be null");
        requiresTarget = targetRequired;
    }

    public void ExecuteAction(Health user, Health target)
    {
        if (user == null) 
        {
            Debug.LogError("User is null while executing action.");
            return;
        }
        if (requiresTarget && target == null)
        {
            Debug.LogError("target is required but is null.");
            return;
        }

        actionEffect?.Invoke(user, target);

        if (AttackAnimaticUI.Instance != null)
        {
            Sprite attackerSprite = user.combatSprite;
            Sprite victimSprite = target != null ? target.combatSprite : null;
            AttackAnimaticUI.Instance.PlayAnimatic(attackerSprite, victimSprite);
        }
        else
        {
            Debug.LogWarning("Attackanimatic null");
        }
    }
}
