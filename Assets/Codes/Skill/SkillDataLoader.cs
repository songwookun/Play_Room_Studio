using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SkillData
{
    public float attackInterval;
    public float skillDurat;
    public float damage;

    public string effectType;
    public float effectDuration;
    public float tickDamage;
    public float speedReduction;

    public int cost; 
}


public static class SkillDataLoader
{
    public static Dictionary<int, SkillData> LoadSkillDatas()
    {
        string filePath = Application.dataPath + "/Datafile/SkillData.csv";
        Dictionary<int, SkillData> skillDatas = new Dictionary<int, SkillData>();

        if (!File.Exists(filePath))
        {
            Debug.LogError("SkillData.csv ������ Assets/Datafile/ ������ �����ϴ�!");
            return skillDatas;
        }

        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] parts = lines[i].Split(',');

            if (parts.Length < 8)
            {
                Debug.LogWarning($"SkillData.csv {i + 1}��° �ٿ� �ʵ尡 �����մϴ�. �ǳʶݴϴ�. ({parts.Length}/8)");
                continue;
            }

            try
            {
                int id = int.Parse(parts[0]);

                SkillData data = new SkillData
                {
                    attackInterval = float.Parse(parts[1]),
                    skillDurat = float.Parse(parts[2]),
                    damage = float.Parse(parts[3]),
                    effectType = parts[4],
                    effectDuration = float.Parse(parts[5]),
                    tickDamage = float.Parse(parts[6]),
                    speedReduction = float.Parse(parts[7]),
                    cost = int.Parse(parts[8]) // ?? cost �߰�
                };


                skillDatas[id] = data;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($" SkillData.csv {i + 1}��° �� �Ľ� �� ����: {ex.Message}");
            }
        }

        return skillDatas;
    }
}
