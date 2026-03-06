using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public GameObject startMenu;
    public GameObject stageMenu;
    public GameObject gameUI;
    public GameObject gameClearUI;
    public GameObject gameOverUI;
    public GameObject quitUI;

    [Header("Timer")]
    public TextMeshProUGUI timerText;
    float timeRemaining;
    bool timerRunning;

    [Header("Stage")]
    public StageManager stageManager;

    int enemyCount;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ShowStartMenu();
    }

    void Update()
    {
        TimerUpdate();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameUI.activeSelf)
            {
                PauseGame();
            }
        }
    }

    void TimerUpdate()
    {
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;

        int min = Mathf.FloorToInt(timeRemaining / 60);
        int sec = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = $"{min:00}:{sec:00}";

        if (timeRemaining <= 0)
        {
            GameOver();
        }
    }

    public void ShowStartMenu()
    {
        startMenu.SetActive(true);
        stageMenu.SetActive(false);
        gameUI.SetActive(false);
        gameClearUI.SetActive(false);
        gameOverUI.SetActive(false);
        quitUI.SetActive(false);

        Time.timeScale = 0;
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        stageMenu.SetActive(true);
    }

    public void StartStage1()
    {
        stageMenu.SetActive(false);
        gameUI.SetActive(true);

        Time.timeScale = 1;

        stageManager.StartStage(1);

        enemyCount = 5;

        timeRemaining = 300f;
        timerRunning = true;
    }

    public void EnemyKilled()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            GameClear();
        }
    }

    void GameClear()
    {
        timerRunning = false;
        gameClearUI.SetActive(true);
        Time.timeScale = 0;

        Invoke(nameof(ReturnStart), 3f);
    }

    public void GameOver()
    {
        timerRunning = false;
        gameOverUI.SetActive(true);
        Time.timeScale = 0;

        Invoke(nameof(ReturnStart), 3f);
    }

    void ReturnStart()
    {
        ShowStartMenu();
    }

    public void PauseGame()
    {
        quitUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void QuitToMenu()
    {
        quitUI.SetActive(false);
        ShowStartMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}