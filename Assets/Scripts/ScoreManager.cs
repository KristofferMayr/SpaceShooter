using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public static event Action<int> OnScoreChanged;
    
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI scoreText; // Einziges Textfeld für beide Anzeigen
    private EnemySpawner enemySpawner;
    private AsteroidSpawner asteroidSpawner;
    private int currentScore = 0;
    private int highscore = 0;
    private string playerName = "Player";

    void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("Load SaveGame");
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighscore();
            UpdateScoreDisplay();
        }
        else
        {
            Destroy(gameObject);
        }
        enemySpawner = FindObjectOfType<EnemySpawner>();
        asteroidSpawner = FindObjectOfType<AsteroidSpawner>();
    }

    private void LoadHighscore()
    {
        highscore = SaveSystem.LoadScore(out playerName);
        currentScore = 0;
        UpdateScoreDisplay();
    }

    public void AddScore(int points)
    {
        currentScore += points;
        OnScoreChanged?.Invoke(currentScore);
        
        if (currentScore > highscore)
        {
            highscore = currentScore;
            SaveSystem.SaveScore(highscore, playerName);
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
            Debug.Log("Update Score Current: " + currentScore + " High: " + highscore);
            // Kombinierte Anzeige in einem Textfeld
            scoreText.text = $"SCORE: <color=#FFD700>{currentScore}</color>\n" + $"BEST: <color=#FF00FF>{highscore}</color>";
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
        if (currentScore >= highscore)
        {
            SaveSystem.SaveScore(highscore, playerName);
        }
    }
}