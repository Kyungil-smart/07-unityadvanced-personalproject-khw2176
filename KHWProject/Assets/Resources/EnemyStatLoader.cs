using UnityEngine;
using System.IO;

public static class EnemyStatLoader
{
    public static (float hp, float damage, float time) Load(int stage)
    {
        TextAsset csv = Resources.Load<TextAsset>("StageData");

        if (csv == null)
        {
            Debug.LogError("StageData.csv missing!");
            return (3, 1, 300);
        }

        StringReader reader = new StringReader(csv.text);
        reader.ReadLine();

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] v = line.Split(',');

            if (int.Parse(v[0]) == stage)
            {
                return (
                    float.Parse(v[1]),
                    float.Parse(v[2]),
                    float.Parse(v[3])
                );
            }
        }

        return (3, 1, 300);
    }
}