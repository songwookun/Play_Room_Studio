using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class NormalAttackDataLoader
{
    public static Dictionary<int, NormalAttackData> LoadData()
    {
        string path = Application.dataPath + "/Datafile/NormalAttackData.csv";
        Dictionary<int, NormalAttackData> data = new Dictionary<int, NormalAttackData>();

        if (!File.Exists(path))
        {
            Debug.LogError("NormalAttackData.csv 파일이 없습니다!");
            return data;
        }

        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            string[] parts = lines[i].Split(',');

            NormalAttackData entry = new NormalAttackData
            {
                id = int.Parse(parts[0]),
                attackInte = float.Parse(parts[1]),
                slashDurat = float.Parse(parts[2]),
                damge = float.Parse(parts[3])
            };

            data[entry.id] = entry;
        }

        return data;
    }
}
