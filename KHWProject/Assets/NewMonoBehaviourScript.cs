using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectUI : MonoBehaviour
{
    public GameObject pausePopup;
    public GameObject gameOverPopup;
    public GameObject stageClearPopup;

    bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Pause On/Off
    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePopup.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    // Continue 버튼
    public void ContinueGame()
    {
        isPaused = false;
        pausePopup.SetActive(false);
        Time.timeScale = 1;
    }

    // Title 이동
    public void GoToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // Game Over
    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        gameOverPopup.SetActive(true);
    }

    // Stage Clear
    public void ShowStageClear()
    {
        Time.timeScale = 0f;
        stageClearPopup.SetActive(true);
    }

    public enum StateList
    {
        Idle, Move, Jump, Attack
    }
}
