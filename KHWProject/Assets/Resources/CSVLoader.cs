using UnityEngine;
using System.Collections.Generic;

public class CSVLoader : MonoBehaviour
{
    public TextAsset enemyCSV;

    public Dictionary<int, (float hp, float damage)> LoadEnemyData()
    {
        Dictionary<int, (float, float)> data = new();

        string[] lines = enemyCSV.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(',');
            int stage = int.Parse(row[0]);
            float hp = float.Parse(row[1]);
            float damage = float.Parse(row[2]);

            data.Add(stage, (hp, damage));
        }
        return data;
    }
}