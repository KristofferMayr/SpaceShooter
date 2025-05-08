using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject panel; // Weiß du im Inspector zu (z. B. PausePanel)
    private bool isPaused = false;
    
    // Referenzen zu den Buttons
    public Button resumeButton;
    public Button optionsButton;
    public Button otherLevelButton;
    public Button quitButton;
    
    void Start()
    {
        // Setze die Button-Listeners
        if (resumeButton != null)
            resumeButton.onClick.AddListener(TogglePause);
            
        if (optionsButton != null)
            optionsButton.onClick.AddListener(OpenOptions);
            
        if (otherLevelButton != null)
            otherLevelButton.onClick.AddListener(LoadOtherLevel);
            
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
            
        // Stelle sicher, dass das Pausemenü beim Start nicht angezeigt wird
        if (panel != null)
            panel.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            Time.timeScale = 0f;
            panel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            panel.SetActive(false);
        }
    }
    
    // Methode für den Options-Button
    public void OpenOptions()
    {
        // Hier implementieren Sie die Logik zum Öffnen des Options-Menüs
        Debug.Log("Options Button geklickt");
    }
    
    // Methode für den Other Level-Button
    public void LoadOtherLevel()
    {
        // Lade die LevelSelectScene
        Debug.Log("Wechsle zur LevelSelectScene");
        Time.timeScale = 1f; // Wichtig: Stelle die Zeitskala wieder her, bevor die Szene gewechselt wird
        SceneManager.LoadScene("LevelSelectScene");
    }
    
    // Methode für den Quit-Button
    public void QuitGame()
    {
        Debug.Log("Spiel wird beendet");
        Application.Quit();
        
        // Im Editor-Modus wird Application.Quit() nicht ausgeführt,
        // daher ist dieser Debug-Log nützlich für Tests
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}