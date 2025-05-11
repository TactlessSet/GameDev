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

        continueButton.onClick.AddListener(() =>
        {
            victoryScreen.SetActive(false);
            currentLevel++;
            StartNextLevel();

            
        });
    }

    public void CheckIfEnemiesDefeated()
    {
        Debug.Log("CheckIfEnemiesDefeated() was called");
        int checkingIndex = Mathf.Clamp(currentLevel - 1, 0, enemyWaves.Count - 1);
        Debug.Log($"Checking enemies in wave index: {checkingIndex}");

        bool allDead = true;

        var enemies = enemyWaves[checkingIndex].GetComponentsInChildren<Health>();
        Debug.Log($"Enemies found: {enemies.Length}");

        foreach (Health enemy in enemies)
        {
            Debug.Log($"Checking {enemy.characterName} with health {enemy.currentHealth}");

            if (enemy.currentHealth > 0 && enemy.CompareTag("Enemy"))
            {
                Debug.Log($"{enemy.characterName} is still alive.");
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
        Debug.Log("Displaying victory screen...");
        victoryScreen.SetActive(true);
    }

    public void StartNextLevel()
    {
        foreach (var wave in enemyWaves)
        {
            wave.SetActive(false);
        }

        if (currentLevel < enemyWaves.Count)
        {
            GameObject wave = enemyWaves[currentLevel];
            wave.SetActive(true);

            EnableEnemyUIInWave(wave);

            if (currentLevel == enemyWaves.Count - 1)
            {
                Debug.Log("Boss wave starting!");
            }

        }
        else
        {
            Debug.Log("All levels complete. Game Over.");
            ShowGameOverScreen();
        }
    }

    private void EnableEnemyUIInWave(GameObject wave)
    {
        var healthComponents = wave.GetComponentsInChildren<Health>(true);

        foreach (var health in healthComponents)
        {
            if (health.gameObject.activeInHierarchy)
            {
                if (health.healthBar != null)
                    health.healthBar.SetActive(true);

                if (health.healthText != null)
                    health.healthText.gameObject.SetActive(true);
            }
        }
    }


    private void ShowGameOverScreen()
    {
        Debug.Log("Game complete!");
        //do later
    }
}
