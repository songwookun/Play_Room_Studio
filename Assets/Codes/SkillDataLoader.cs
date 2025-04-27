using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SkillData
{
    public float attackInterval;
    public float skillDuration;
    public float damage;
}

public class SkillDataLoader
{
    public static Dictionary<int, SkillData> LoadSkillDatas()
    {
        string filePath = Application.dataPath + "/Datafile/SkillData.csv";
        Dictionary<int, SkillData> skillDatas = new Dictionary<int, SkillData>();

        if (!File.Exists(filePath))
        {
            Debug.LogError("SkillData.csv 파일이 Assets/Datafile/ 폴더에 없습니다!");
            return skillDatas;
        }

        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] parts = lines[i].Split(',');

            int id = int.Parse(parts[0]);
            SkillData data = new SkillData
            {
                attackInterval = float.Parse(parts[1]),
                skillDuration = float.Parse(parts[2]),
                damage = float.Parse(parts[3])
            };

            skillDatas[id] = data;
        }

        return skillDatas;
    }
}
