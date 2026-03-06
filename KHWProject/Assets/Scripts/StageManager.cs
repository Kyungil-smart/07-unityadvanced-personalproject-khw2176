using UnityEngine;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    [Header("적 관리")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;

    public int stage = 1;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public void StartStage(int stageNum)
    {
        stage = stageNum;
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        foreach (var spawn in spawnPoints)
        {
            int index = Random.Range(0, enemyPrefabs.Count);
            GameObject enemy = Instantiate(enemyPrefabs[index], spawn.position, spawn.rotation);
            spawnedEnemies.Add(enemy);
        }
    }

    // 게임 시작 시 아이템 생성 코드 제거!
}