using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ActionPanelUI : MonoBehaviour
{
    public Button[] actionButtons;

    private CharacterAction[] currentActions;

    public void ShowActions(List<CharacterAction> actions)
    {
        gameObject.SetActive(true);
        Debug.Log("Showing actions...");
        currentActions = actions.ToArray();

        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (i < currentActions.Length)
            {
                var action = currentActions[i];
                Debug.Log($"Setting button {i} active with action {currentActions[i].actionName}");
                actionButtons[i].gameObject.SetActive(true);
                actionButtons[i].GetComponentInChildren<TMP_Text>().text = action.actionName;

                int index = i;
                actionButtons[i].onClick.RemoveAllListeners();
                actionButtons[i].onClick.AddListener(() => OnActionSelected(index));
            }
            else
            {
                actionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnActionSelected(int index)
    {
        Debug.Log($"Selected: {currentActions[index].actionName}");
        PlayerCombatController.Instance.PrepareAction(currentActions[index]);
        gameObject.SetActive(false);
    }
}
