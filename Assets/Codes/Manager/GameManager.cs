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

    // �Ͻ����� ���� ����
    private bool isPaused = false;
    public GameObject pausePanel;

    // ��ư �̹��� ��ȯ�� ���� ����
    public Image pauseButtonImage;     // ��ư ���� Image ������Ʈ
    public Sprite stopSprite;          // �Ͻ����� ������ (Stop_0)
    public Sprite startSprite;         // ��� ������ (Start_0)

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
            Debug.LogWarning($"���� {currentLevel}�� ���� ����ġ �����Ͱ� �����ϴ�.");
            return 99999999f; // ���� ������ ��ǻ� ������ �Ұ�
        }
    }

    private void LoadLevelExpData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("LevelExp");
        if (csvFile == null)
        {
            Debug.LogError("LevelExp.csv ������ ã�� �� �����ϴ�! Resources ������ �־�� �մϴ�.");
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
                    Debug.LogWarning($"�߸��� ������ ������ ��: {line}");
                }
            }
        }
    }

    // �Ͻ����� ��� �� �̹��� ���� �Լ�
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pausePanel != null)
            pausePanel.SetActive(isPaused);

        if (pauseButtonImage != null)
            pauseButtonImage.sprite = isPaused ? startSprite : stopSprite;

        Debug.Log(isPaused ? "���� �Ͻ�����" : "���� �簳");
    }
}
