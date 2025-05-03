using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    List<SpawnData> spawnDataList = new List<SpawnData>();
    SpawnData currentData;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        StartCoroutine(LoadSpawnDataFromCSV());
    }

    IEnumerator LoadSpawnDataFromCSV()
    {
        string fileName = "SpawnData.csv";
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string[] lines;

#if UNITY_ANDROID && !UNITY_EDITOR
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("SpawnData.csv 로딩 실패: " + www.error);
            yield break;
        }

        lines = www.downloadHandler.text.Split('\n');
#else
        if (!File.Exists(filePath))
        {
            Debug.LogError("SpawnData.csv 파일 없음: " + filePath);
            yield break;
        }

        lines = File.ReadAllLines(filePath);
#endif

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] parts = lines[i].Trim().Split(',');

            if (parts.Length < 5)
            {
                Debug.LogWarning($"SpawnData.csv {i + 1}줄에 필드 부족: {parts.Length}/5");
                continue;
            }

            SpawnData data = new SpawnData();
            data.startTime = float.Parse(parts[0]);
            data.spriteType = int.Parse(parts[1]);
            data.health = int.Parse(parts[2]);
            data.speed = float.Parse(parts[3]);
            data.spawnTime = float.Parse(parts[4]);

            spawnDataList.Add(data);
        }

        Debug.Log("SpawnData.csv 로딩 완료: " + spawnDataList.Count + "개");
    }

    void Update()
    {
        timer += Time.deltaTime;
        float gameTime = GameManager.Instance.gameTime;

        foreach (var data in spawnDataList)
        {
            if (gameTime >= data.startTime)
                currentData = data;
        }

        if (currentData == null) return;

        if (timer > currentData.spawnTime)
        {
            timer = 0f;
            Spawn(currentData);
        }
    }

    void Spawn(SpawnData data)
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(data);
    }
}

[System.Serializable]
public class SpawnData
{
    public float startTime;
    public int spriteType;
    public int health;
    public float speed;
    public float spawnTime;
}
