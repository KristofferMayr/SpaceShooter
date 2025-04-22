using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    public int currentLevelIndex = 1;

    public void CompleteLevel()
    {
        LevelManager.CompleteLevel(currentLevelIndex);
        // Optional: zurück zur Level-Auswahl
        // SceneManager.LoadScene("LevelSelectScene");
    }
}