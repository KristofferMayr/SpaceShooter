using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI scoreText;
    private int currentScore = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        scoreText.text = $"SCORE: <color=#FFD700>{currentScore}</color>"; // Goldener Text mit Formatierung
    }

    // Highscore-Logik (optional)
    public void SaveHighscore()
    {
        PlayerPrefs.SetInt("Highscore", currentScore);
        PlayerPrefs.Save();
    }
}