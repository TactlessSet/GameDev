using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject victoryScreen;
    public Button continueButton;

    private int currentLevel = 0;
    public List<GameObject> enemyWaves;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        foreach (var wave in enemyWaves)
        {
            wave.SetActive(false);
        }

        victoryScreen.SetActive(false);
    }

    private void Start()
    {
        StartNextLevel();
        continueButton.onClick.AddListener(() => {
            victoryScreen.SetActive(false);
            StartNextLevel();
        });
    }

    public void CheckIfEnemiesDefeated()
    {
        bool allDead = true;

        int checkingIndex = Mathf.Clamp(currentLevel - 1, 0, enemyWaves.Count - 1);
        foreach (Health enemy in enemyWaves[checkingIndex].GetComponentsInChildren<Health>())
        {
            if (enemy.currentHealth > 0 && enemy.CompareTag("Enemy"))
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            Debug.Log("Wave cleared!");
            ShowVictoryScreen();
        }
    }

    private void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    public void StartNextLevel()
    {
        if (currentLevel < enemyWaves.Count)
        {
            enemyWaves[currentLevel].SetActive(true);

            if (currentLevel == enemyWaves.Count - 1)
            {
                Debug.Log("Boss wave starting!");
            }

            currentLevel++;
        }
        else
        {
            Debug.Log("All levels complete. Game Over.");
            ShowGameOverScreen();
        }
    }

    private void ShowGameOverScreen()
    {
        Debug.Log("Game complete!");
        //need to add a screen, of course
    }

}
