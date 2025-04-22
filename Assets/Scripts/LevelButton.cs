using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Level-Einstellungen")]
    public int levelIndex = 1;
    public string levelSceneName = "Level1";

    [Header("UI")]
    public Button button;

    void Awake()
    {
        // Falls kein Button gesetzt wurde, versuche ihn automatisch zu finden
        if (button == null)
            button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("Kein Button zugewiesen oder gefunden!");
            return;
        }

        // OnClick-Listener hinzufügen
        button.onClick.AddListener(OnButtonClick);

        // Interaktivität setzen
        button.interactable = LevelManager.IsLevelUnlocked(levelIndex);
    }

    public void OnButtonClick()
    {
        Debug.Log($"Button für Level {levelIndex} geklickt → {levelSceneName}");
        FindObjectOfType<LevelManager>().LoadLevel(levelSceneName);
    }
}
