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
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("게임 종료 - Game Over 패널 열림");
    }

    public void OnClickRevive()
    {
        var gm = GameManager.Instance;
        gm.health = gm.maxHealth;

        Time.timeScale = 1f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Debug.Log("부활 - 체력 복구 및 게임 재개");
    }

    public void ShowResult()
    {
        Debug.Log("ShowResult 호출됨");

        if (gameOverPanel != null)
        {
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

    public void OnClickDoubleReward()
    {
        GameManager.Instance.collectedCoins *= 2;

        if (coinText != null)
            coinText.text = $"고대 주화: {GameManager.Instance.collectedCoins}";

        PlayerPrefs.SetInt("LastGold", GameManager.Instance.collectedCoins);
        PlayerPrefs.Save();

        Debug.Log("보상 2배 적용");
    }

    public void OnClickExitToStart()
    {
        Time.timeScale = 1f; // 혹시 멈춰있을 경우를 대비
        SceneManager.LoadScene("StartScene");
        Debug.Log("나가기 버튼 클릭 - StartScene으로 이동");
    }

}
