using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject victoryScreen;
    public GameObject gameOverScreen;
    public Button continueButton;
    public Button returnButton;
    public List<GameObject> enemyWaves;

    private int currentLevel = 0;
    private GameObject currentWaveObject;

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
        continueButton.onClick.AddListener(OnContinuePressed);
        StartCurrentLevel();
    }

    private void OnContinuePressed()
    {
        victoryScreen.SetActive(false);
        currentLevel++;
        StartCurrentLevel();
    }

    public void OnReturnButtonPressed()
    {
        SceneManager.LoadScene(1);
    }

    private void StartCurrentLevel()
    {
        if (currentLevel >= enemyWaves.Count)
        {
            Debug.Log("All levels complete. Game Over.");
            ShowGameOverScreen();
            return;
        }

        DisableAllEnemyUI();
        foreach (var wave in enemyWaves)
        {
            wave.SetActive(false);
        }

        currentWaveObject = enemyWaves[currentLevel];
        currentWaveObject.SetActive(true);
        EnableEnemyUIInWave(currentWaveObject);

        if (currentLevel == enemyWaves.Count - 1)
        {
            Debug.Log("Boss wave starting!");
        }
    }

    public void CheckIfEnemiesDefeated()
    {
        if (currentWaveObject == null)
        {
            Debug.LogWarning("No current wave to check!");
            return;
        }

        bool allDead = true;
        var enemies = currentWaveObject.GetComponentsInChildren<Health>(true);

        foreach (Health enemy in enemies)
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
            ShowGameOverScreen();
        }
    }

    private void ShowVictoryScreen()
    {
        Debug.Log("Displaying victory screen...");
        victoryScreen.SetActive(true);
    }

    private void ShowGameOverScreen()
    {
        Debug.Log("Game complete!");
        gameOverScreen.SetActive(true);
    }

    private void DisableAllEnemyUI()
    {
        foreach (var wave in enemyWaves)
        {
            var healthComponents = wave.GetComponentsInChildren<Health>(true);
            foreach (var health in healthComponents)
            {
                if (health.healthBar != null)
                    health.healthBar.SetActive(false);

                if (health.healthText != null)
                    health.healthText.gameObject.SetActive(false);
            }
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
}
