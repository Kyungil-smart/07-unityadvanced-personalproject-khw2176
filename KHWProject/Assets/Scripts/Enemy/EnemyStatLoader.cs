using UnityEngine;
using System.IO;

public static class EnemyStatLoader
{
    public static (float hp, float damage) LoadStat(int stage)
    {
        TextAsset csv = Resources.Load<TextAsset>("StageData");

        StringReader reader = new StringReader(csv.text);
        reader.ReadLine(); // 헤더 제거

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');

            int stageNum = int.Parse(values[0]);

            if (stageNum == stage)
            {
                float hp = float.Parse(values[1]);
                float damage = float.Parse(values[2]);
                return (hp, damage);
            }
        }

        return (3, 1);
    }
}