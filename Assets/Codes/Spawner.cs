using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    List<SpawnData> spawnDataList = new List<SpawnData>();
    SpawnData currentData;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        LoadSpawnDataFromCSV();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 현재 시간에 맞는 몬스터 데이터 선택
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

    void LoadSpawnDataFromCSV()
    {
        string filePath = Application.dataPath + "/Datafile/SpawnData.csv";

        if (!File.Exists(filePath))
        {
            Debug.LogError("SpawnData.csv 파일이 Assets/Datafile/ 폴더에 없습니다!");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] parts = lines[i].Trim().Split(',');

            SpawnData data = new SpawnData();
            data.startTime = float.Parse(parts[0]);
            data.spriteType = int.Parse(parts[1]);
            data.health = int.Parse(parts[2]);
            data.speed = float.Parse(parts[3]);
            data.spawnTime = float.Parse(parts[4]);

            spawnDataList.Add(data);
        }
    }
}

[System.Serializable]
public class SpawnData
{
    public float startTime;   // 몇 초부터 등장할지
    public int spriteType;
    public int health;
    public float speed;
    public float spawnTime;   // 등장 주기
}
