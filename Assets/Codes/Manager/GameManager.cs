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

    public int collectedCoins = 0;
    public int collectedMP = 0;

    private Dictionary<int, float> levelExpDict = new Dictionary<int, float>();

    // �Ͻ����� ���� ����
    private bool isPaused = false;
    public GameObject pausePanel;
    public Image pauseButtonImage;
    public Sprite stopSprite;
    public Sprite startSprite;

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

        //ü�� 0�� �� GameOver ȣ��
        CheckGameOver();
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
            return 99999999f;
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

    // ��� ������ ó�� �Լ���
    public void GainCoin(int amount = 1)
    {
        collectedCoins += amount;
    }

    public void GainMP(int amount = 1)
    {
        collectedMP += amount;
    }

    public void Heal(float percent)
    {
        health = Mathf.Min(health + (int)(maxHealth * percent), maxHealth); // float �� int ��ȯ
    }

    // �Ͻ����� ��� �� �̹��� ����
    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel.SetActive(isPaused);
        pauseButtonImage.sprite = isPaused ? startSprite : stopSprite;

        // �Է�/�̵� ����
        player.enabled = !isPaused;

        // �ִϸ����� ����
        player.GetComponent<Animator>().enabled = !isPaused;

        // ��ü ���� �Ͻ�����
        AudioListener.pause = isPaused;

        Debug.Log(isPaused ? "���� ���� ����" : "���� �簳");
    }

    public void OnClickResumeButton()
    {
        // ���� �Ͻ����� ����
        isPaused = false;
        Time.timeScale = 1f;

        // �˾� �����
        if (pausePanel != null)
            pausePanel.SetActive(false);

        // ���� ��ư �̹��� ���� 
        if (pauseButtonImage != null)
            pauseButtonImage.sprite = stopSprite;

        // �÷��̾� ��ũ��Ʈ �ٽ� Ȱ��ȭ
        if (player != null)
        {
            player.enabled = true;

            Animator anim = player.GetComponent<Animator>();
            if (anim != null)
                anim.enabled = true;
        }
    }

    // ü�� 0�̸� GameOver ȣ��
    public void CheckGameOver()
    {
        if (health <= 0 && !UIManager.Instance.gameOverPanel.activeSelf)
        {
            UIManager.Instance.ShowGameOver();
        }
    }
}
