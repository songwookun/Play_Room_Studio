using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class NormalAttackDataLoader
{
    public static Dictionary<int, NormalAttackData> normalAttackData = new Dictionary<int, NormalAttackData>();

    public static IEnumerator LoadData(System.Action onComplete = null)
    {
        string fileName = "NormalAttackData.csv";

#if UNITY_ANDROID && !UNITY_EDITOR
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("NormalAttackData.csv 로딩 실패: " + www.error);
            yield break;
        }

        string[] lines = www.downloadHandler.text.Split('\n');
#else
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError("NormalAttackData.csv 파일 없음: " + filePath);
            yield break;
        }

        string[] lines = File.ReadAllLines(filePath);
#endif

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] parts = lines[i].Split(',');
            if (parts.Length < 4) continue;

            NormalAttackData entry = new NormalAttackData
            {
                id = int.Parse(parts[0]),
                attackInte = float.Parse(parts[1]),
                slashDurat = float.Parse(parts[2]),
                damge = float.Parse(parts[3])
            };

            normalAttackData[entry.id] = entry;
        }

        onComplete?.Invoke();
    }
}