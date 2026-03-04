using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [SerializeField] int stageIndex;
    [SerializeField] float limitTime;

    int enemyCount;
    int damageItemRemain;
    int healItemRemain;

    void Awake()
    {
        Instance = this;
    }

    public void SetupStage()
    {
        if (stageIndex == 1)
        {
            enemyCount = 5;
            damageItemRemain = 3;
            healItemRemain = 2;
            limitTime = 300;
        }
        else if (stageIndex == 2)
        {
            enemyCount = 7;
            damageItemRemain = 4;
            healItemRemain = 3;
            limitTime = 360;
        }
        else
        {
            enemyCount = 9;
            damageItemRemain = 4;
            healItemRemain = 4;
            limitTime = 420;
        }
    }

    public void EnemyKilled(Vector3 pos)
    {
        enemyCount--;

        if (ItemFactory.Instance != null)
        {
            if (Random.value > 0.5f && damageItemRemain > 0)
            {
                ItemFactory.Instance.SpawnDamage(pos);
                damageItemRemain--;
            }
            else if (healItemRemain > 0)
            {
                ItemFactory.Instance.SpawnHeal(pos);
                healItemRemain--;
            }
        }

        if (enemyCount <= 0)
            NextStage();
    }

    void NextStage()
    {
        if (stageIndex == 3)
            SceneManager.LoadScene("ClearScene");
        else
            SceneManager.LoadScene("Stage" + (stageIndex + 1));
    }
}