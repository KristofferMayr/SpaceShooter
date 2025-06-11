using System.Collections;
using UnityEngine;
using TMPro; // Wichtig: TextMeshPro Namespace einbinden
using UnityEngine.SceneManagement;

public class TextSwitcher : MonoBehaviour
{
    public TextMeshProUGUI displayText;  // TMP Text-Komponente

    public string[] textsToShow;          // Texte, die angezeigt werden sollen
    public float[] displayDurations;      // Anzeigezeiten je Text (Sekunden)
    public string nextSceneName;          // Name der Szene, die danach geladen wird

    void Start()
    {
        StartCoroutine(ShowTextsCoroutine());
    }

    IEnumerator ShowTextsCoroutine()
    {
        for (int i = 0; i < textsToShow.Length; i++)
        {
            displayText.text = textsToShow[i];
            yield return new WaitForSeconds(displayDurations[i]);
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
