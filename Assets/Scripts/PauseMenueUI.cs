using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject panel; // Weist du im Inspector zu (z. B. PausePanel)
    private bool isPaused = false;

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
}
