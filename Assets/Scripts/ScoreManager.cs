using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public static event Action<int> OnScoreChanged;
    
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private EnemySpawner enemySpawner;
    private AsteroidSpawner asteroidSpawner;
    
    private int currentScore = 0;
    private int currentLevelHighscore = 0;
    private string playerName = "Player";
    private int currentLevelIndex = 1; // Standardmäßig Level 1

    void Awake()
    {
        if (PlayerPrefs.HasKey("CurrentLevelIndex"))
        {
            SetCurrentLevel(PlayerPrefs.GetInt("CurrentLevelIndex"));
        }

        if (Instance == null)
        {
            Debug.Log("Load SaveGame");
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLevelHighscore();
            UpdateScoreDisplay();
        }
        else
        {
            Destroy(gameObject);
        }
        enemySpawner = FindObjectOfType<EnemySpawner>();
        asteroidSpawner = FindObjectOfType<AsteroidSpawner>();
    }

    // Setzt das aktuelle Level (sollte beim Levelstart aufgerufen werden)
    public void SetCurrentLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        LoadLevelHighscore();
    }

    private void LoadLevelHighscore()
    {
        currentLevelHighscore = SaveSystem.LoadScore(currentLevelIndex, out playerName);
        currentScore = 0;
        UpdateScoreDisplay();
    }

    public void AddScore(int points)
    {
        currentScore += points;
        OnScoreChanged?.Invoke(currentScore);
        
        if (currentScore > currentLevelHighscore)
        {
            currentLevelHighscore = currentScore;
            SaveSystem.SaveScore(currentLevelIndex, currentLevelHighscore, playerName);
        }
        
        UpdateScoreDisplay();
        
        // Boss spawnen bei Score 100
        if (currentScore == 100 && enemySpawner != null)
        {
            enemySpawner.SpawnBoss();
            asteroidSpawner.spawnAsteroids = false; 
        }
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            Debug.Log($"Update Score - Level: {currentLevelIndex} Current: {currentScore} High: {currentLevelHighscore}");
            scoreText.text = $"LEVEL: {currentLevelIndex}\n" +
                            $"SCORE: <color=#FFD700>{currentScore}</color>\n" + 
                            $"BEST: <color=#FF00FF>{currentLevelHighscore}</color>";
        }
    }

    public void ResetCurrentScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }

    public void SetPlayerName(string newName)
    {
        playerName = newName;
        if (currentScore >= currentLevelHighscore)
        {
            SaveSystem.SaveScore(currentLevelIndex, currentLevelHighscore, playerName);
        }
    }

    // Gibt den Highscore für ein bestimmtes Level zurück (für Levelauswahl-UI etc.)
    public int GetHighscoreForLevel(int levelIndex)
    {
        return SaveSystem.LoadScore(levelIndex, out _);
    }
}