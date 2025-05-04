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

    // 일시정지 관련 변수
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

        //체력 0일 때 GameOver 호출
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
            Debug.LogWarning($"레벨 {currentLevel}에 대한 경험치 데이터가 없습니다.");
            return 99999999f;
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

    // 드랍 아이템 처리 함수들
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
        health = Mathf.Min(health + (int)(maxHealth * percent), maxHealth); // float → int 변환
    }

    // 일시정지 토글 및 이미지 변경
    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel.SetActive(isPaused);
        pauseButtonImage.sprite = isPaused ? startSprite : stopSprite;

        // 입력/이동 차단
        player.enabled = !isPaused;

        // 애니메이터 끄기
        player.GetComponent<Animator>().enabled = !isPaused;

        // 전체 사운드 일시정지
        AudioListener.pause = isPaused;

        Debug.Log(isPaused ? "게임 완전 정지" : "게임 재개");
    }

    public void OnClickResumeButton()
    {
        // 게임 일시정지 해제
        isPaused = false;
        Time.timeScale = 1f;

        // 팝업 숨기기
        if (pausePanel != null)
            pausePanel.SetActive(false);

        // 정지 버튼 이미지 복원 
        if (pauseButtonImage != null)
            pauseButtonImage.sprite = stopSprite;

        // 플레이어 스크립트 다시 활성화
        if (player != null)
        {
            player.enabled = true;

            Animator anim = player.GetComponent<Animator>();
            if (anim != null)
                anim.enabled = true;
        }
    }

    // 체력 0이면 GameOver 호출
    public void CheckGameOver()
    {
        if (health <= 0 && !UIManager.Instance.gameOverPanel.activeSelf)
        {
            UIManager.Instance.ShowGameOver();
        }
    }
}
