using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("현재 스테이지")]
    [Tooltip("현재 진행중인 스테이지 번호")]
    public int currentStage = 1;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void LoadStage(int stage)
    {
        currentStage = stage;
        SceneManager.LoadScene("Stage" + stage);
    }

    public void LoadMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}