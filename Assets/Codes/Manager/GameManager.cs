using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameTime;
    public float maxGmaeTime;

    public PoolManger pool;
    public PlayerMove player;
    public int health;
    public int maxHealth = 100;
    public float exp;
    public int level;
    public float Kill;

    private Dictionary<int, float> levelExpDict = new Dictionary<int, float>();

    // 일시정지 관련 변수
    private bool isPaused = false;
    public GameObject pausePanel;

    // 버튼 이미지 전환을 위한 변수
    public Image pauseButtonImage;     // 버튼 안의 Image 컴포넌트
    public Sprite stopSprite;          // 일시정지 아이콘 (Stop_0)
    public Sprite startSprite;         // 재생 아이콘 (Start_0)

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        health = maxHealth;
        LoadLevelExpData();
    }

    void Update()
    {
        gameTime += Time.deltaTime;
    }

    public void GainExp(float amount)
    {
        exp += amount;

        while (exp >= GetNextExp(level))
        {
            exp -= GetNextExp(level);
            level++;
        }
    }

    public float GetNextExp(int currentLevel)
    {
        if (levelExpDict.ContainsKey(currentLevel))
        {
            return levelExpDict[currentLevel];
        }
        else
        {
            Debug.LogWarning($"레벨 {currentLevel}에 대한 경험치 데이터가 없습니다.");
            return 99999999f; // 없는 레벨은 사실상 레벨업 불가
        }
    }

    private void LoadLevelExpData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("LevelExp");
        if (csvFile == null)
        {
            Debug.LogError("LevelExp.csv 파일을 찾을 수 없습니다! Resources 폴더에 있어야 합니다.");
            return;
        }

        StringReader reader = new StringReader(csvFile.text);

        bool isFirstLine = true;
        while (true)
        {
            string line = reader.ReadLine();
            if (line == null) break;

            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            string[] split = line.Split(',');
            if (split.Length >= 2)
            {
                if (int.TryParse(split[0], out int level) && float.TryParse(split[1], out float requiredExp))
                {
                    levelExpDict[level] = requiredExp;
                }
                else
                {
                    Debug.LogWarning($"잘못된 형식이 감지된 줄: {line}");
                }
            }
        }
    }

    // 일시정지 토글 및 이미지 변경 함수
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pausePanel != null)
            pausePanel.SetActive(isPaused);

        if (pauseButtonImage != null)
            pauseButtonImage.sprite = isPaused ? startSprite : stopSprite;

        Debug.Log(isPaused ? "게임 일시정지" : "게임 재개");
    }
}
