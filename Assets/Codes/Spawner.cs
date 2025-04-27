using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // 파일 읽기
using System.Linq; // 문자열 Split

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    List<SpawnData> spawnDataList = new List<SpawnData>(); 
    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();

        // CSV 파일 직접 읽기 (Datafile 폴더에서)
        LoadSpawnDataFromCSV();
    }

    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.FloorToInt(GameManager.Instance.gameTime / 10f);

        // level이 spawnDataList 범위를 초과하지 않게 고정
        level = Mathf.Min(level, spawnDataList.Count - 1);

        if (timer > spawnDataList[level].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnDataList[level]);
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
            data.spawnTime = float.Parse(parts[0]);
            data.spriteType = int.Parse(parts[1]);
            data.health = int.Parse(parts[2]);
            data.speed = float.Parse(parts[3]);

            spawnDataList.Add(data);
        }
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
