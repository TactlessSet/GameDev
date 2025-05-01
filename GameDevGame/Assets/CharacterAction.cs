using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterAction
{
    public string actionName;
    // Change Action<Health> to Action<Health, Health> to accept both the user and the target
    public System.Action<Health, Health> actionEffect;

    public CharacterAction(string name, System.Action<Health, Health> effect)
    {
        actionName = name;
        actionEffect = effect;
    }

    public void ExecuteAction(Health user, Health target)
    {
        actionEffect?.Invoke(user, target);
    }
}
