using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI scoreText; // Einziges Textfeld für beide Anzeigen

    private int currentScore = 0;
    private int highscore = 0;
    private string playerName = "Player";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighscore();
            UpdateScoreDisplay();
        }
        else
        {
            Destroy(gameObject);
        }
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
        
        if (currentScore > highscore)
        {
            highscore = currentScore;
            SaveSystem.SaveScore(highscore, playerName);
        }
        
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            // Kombinierte Anzeige in einem Textfeld
            scoreText.text = $"SCORE: <color=#FFD700>{currentScore}</color>\n" +
                            $"BEST: <color=#FF00FF>{highscore}</color>";
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