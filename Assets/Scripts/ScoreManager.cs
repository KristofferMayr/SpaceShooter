using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] public TextMeshProUGUI scoreText;
    private int currentScore = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeScore(); // Score beim ersten Laden initialisieren
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeScore()
    {
        // Lade den Score beim Start
        currentScore = SaveSystem.LoadScore(out _);
        UpdateScoreDisplay(currentScore); // UI sofort aktualisieren
    }

    public void AddScore(int points)
    {
        currentScore += points;
        SaveSystem.SaveScore(currentScore, "Player");
        UpdateScoreDisplay(currentScore); // UI nach jeder Änderung aktualisieren
    }

    public void UpdateScoreDisplay(int scoreToDisplay)
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: <color=#FFD700>{scoreToDisplay}</color>";
        }
        else
        {
            Debug.LogWarning("ScoreText-Referenz fehlt!");
        }
    }

    // Für externe Zugriffe (z.B. GameManager)
    public int GetCurrentScore()
    {
        return currentScore;
    }
}