using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

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
    public float PlayerSpeed;
}

public static class SkillDataLoader
{
    public static Dictionary<int, SkillData> skillDatas = new Dictionary<int, SkillData>();

    public static IEnumerator LoadSkillDatas(System.Action onComplete = null)
    {
        string fileName = "SkillData.csv";

#if UNITY_ANDROID && !UNITY_EDITOR
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("SkillData.csv 로딩 실패: " + www.error);
            yield break;
        }

        string[] lines = www.downloadHandler.text.Split('\n');
#else
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError("SkillData.csv 파일 없음: " + filePath);
            yield break;
        }

        string[] lines = File.ReadAllLines(filePath);
#endif

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] parts = lines[i].Trim().Split(',');
            if (parts.Length < 10) continue; // PlayerSpeed 포함

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
                    cost = int.Parse(parts[8]),
                    PlayerSpeed = float.Parse(parts[9]) 
                };
                skillDatas[id] = data;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"SkillData 파싱 오류 (줄 {i + 1}): {ex.Message}");
            }
        }

        onComplete?.Invoke();
    }
}
