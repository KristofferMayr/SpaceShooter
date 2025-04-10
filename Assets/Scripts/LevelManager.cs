using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        // Fortschritt laden (damit SaveSystem initialisiert ist)
        Debug.Log("Höchstes freigeschaltetes Level: " + SaveSystem.GetHighestUnlockedLevel());
    }

    // Lädt ein Level anhand des Namens
    public void LoadLevel(string levelName)
    {
        Debug.Log("Loading Level Scene");
        SceneManager.LoadScene(levelName);
    }

    // Gibt zurück, ob ein Level freigeschaltet ist
    public static bool IsLevelUnlocked(int levelIndex)
    {
        return SaveSystem.IsLevelUnlocked(levelIndex);
    }

    // Schaltet das nächste Level frei
    public static void CompleteLevel(int currentLevelIndex)
    {
        int nextLevelIndex = currentLevelIndex + 1;
        SaveSystem.UnlockLevel(nextLevelIndex);
        Debug.Log($"Level {nextLevelIndex} freigeschaltet!");
    }

    // Für Debug-Zwecke: Fortschritt löschen
    public void ResetProgress()
    {
        SaveSystem.DeleteSave();
        Debug.Log("Fortschritt zurückgesetzt!");
    }
}
