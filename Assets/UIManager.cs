using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Game Over & Revive")]
    public GameObject gameOverPanel;

    [Header("Result")]
    public GameObject resultPanel;
    public Text timeText;
    public Text killText;
    public Text coinText;

    private void Awake()
    {
        // 중복 방지
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 게임 오버 시 호출
    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("게임 종료 - Game Over 패널 열림");
    }

    // 부활 버튼
    public void OnClickRevive()
    {
        var gm = GameManager.Instance;
        gm.health = gm.maxHealth;

        Time.timeScale = 1f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Debug.Log("부활 - 체력 복구 및 게임 재개");
    }

    // 결과 패널 표시
    public void ShowResult()
    {
        Debug.Log("ShowResult 호출됨");

        if (gameOverPanel != null)
        {
            // GameOverPanel의 자식 오브젝트들만 꺼줌
            foreach (Transform child in gameOverPanel.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        if (resultPanel != null)
            resultPanel.SetActive(true);

        var gm = GameManager.Instance;
        int totalSeconds = (int)gm.gameTime;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        if (timeText != null)
            timeText.text = $"플레이 시간: {minutes:00}:{seconds:00}";

        if (killText != null)
            killText.text = $"잡은 몬스터: {gm.Kill}";

        if (coinText != null)
            coinText.text = $"고대 주화: {gm.collectedCoins}";

        PlayerPrefs.SetInt("LastGold", gm.collectedCoins);
        PlayerPrefs.Save();
    }

    // 2배 보상 버튼
    public void OnClickDoubleReward()
    {
        GameManager.Instance.collectedCoins *= 2;

        if (coinText != null)
            coinText.text = $"고대 주화: {GameManager.Instance.collectedCoins}";

        PlayerPrefs.SetInt("LastGold", GameManager.Instance.collectedCoins);
        PlayerPrefs.Save();

        Debug.Log("보상 2배 적용");
    }

    // 시작 화면으로 이동
    public void OnClickGoToStart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }
}
