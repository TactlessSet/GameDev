using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ActionPanelUI : MonoBehaviour
{
    public Button[] actionButtons;

    private CharacterAction[] currentActions;

    // Show actions available for the current character
    public void ShowActions(List<CharacterAction> actions)
    {
        Debug.Log($"ShowActions called with {actions.Count} actions.");
        Debug.Log($"ActionButtons length: {actionButtons.Length}");

        // Ensure the panel is shown
        gameObject.SetActive(true);

        // Store actions for later
        currentActions = actions.ToArray();

        // Loop through buttons and assign actions
        for (int i = 0; i < actionButtons.Length; i++)
        {
            // If there is an action available for the current button index
            if (i < currentActions.Length)
            {
                var action = currentActions[i];
                Debug.Log($"Setting button {i} active with action {currentActions[i].actionName}");

                // Show the button and set its text
                actionButtons[i].gameObject.SetActive(true);
                actionButtons[i].GetComponentInChildren<TMP_Text>().text = action.actionName;

                // Remove previous listeners and add new ones
                int index = i;
                actionButtons[i].onClick.RemoveAllListeners();
                actionButtons[i].onClick.AddListener(() => OnActionSelected(index));
            }
            else
            {
                // Hide any extra buttons if actions are fewer
                actionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // When an action is selected
    void OnActionSelected(int index)
    {
        Debug.Log($"Selected: {currentActions[index].actionName}");
        
        // Pass the selected action to the controller for further processing
        PlayerCombatController.Instance.PrepareAction(currentActions[index]);

        // Hide the action panel after selection
        gameObject.SetActive(false);
    }

    // Optional: Add method to hide the action panel if needed
    public void HideActionPanel()
    {
        gameObject.SetActive(false);
    }
}
