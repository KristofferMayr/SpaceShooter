using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentScore;

    private ScoreManager scoreManager;

    private void Awake()
    {
        Instance = this;
        scoreManager = FindObjectOfType<ScoreManager>(); // Finde den ScoreManager
        LoadGameData();
    }

    private void LoadGameData()
    {
        currentScore = SaveSystem.LoadScore(out _);
    }

    // Aktualisiert die UI über den ScoreManager
}