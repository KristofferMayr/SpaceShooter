using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public SpaceShip player; // Referenz zum Spieler
    public GameObject[] heartPrefabs; // Array von Herz-Prefabs

    // Start is called before the first frame update
    void Start()
    {
        // Initialisiere die Sichtbarkeit der Herzen basierend auf der Gesundheit des Spielers
        UpdateHeartsVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        // Aktualisiere die Sichtbarkeit der Herzen, wenn sich die Gesundheit des Spielers ändert
        UpdateHeartsVisibility();
    }

    private void UpdateHeartsVisibility()
    {
        // Überprüfe die aktuelle Gesundheit des Spielers
        if (player.currentHealth == 3)
        {
            // Setze alle Herzen auf sichtbar
            SetHeartsVisibility(true, true, true);
        }
        else if (player.currentHealth == 2)
        {
            // Setze zwei Herzen auf sichtbar und eines auf unsichtbar
            SetHeartsVisibility(true, true, false);
        }
        else if (player.currentHealth == 1)
        {
            // Setze ein Herz auf sichtbar und zwei auf unsichtbar
            SetHeartsVisibility(true, false, false);
        }
        else if (player.currentHealth == 0)
        {
            // Setze alle Herzen auf unsichtbar
            SetHeartsVisibility(false, false, false);
        }
    }

    private void SetHeartsVisibility(bool heart1Visible, bool heart2Visible, bool heart3Visible)
    {
        // Setze die Sichtbarkeit der Herzen basierend auf den übergebenen Parametern
        heartPrefabs[0].SetActive(heart1Visible);
        heartPrefabs[1].SetActive(heart2Visible);
        heartPrefabs[2].SetActive(heart3Visible);
    }
}