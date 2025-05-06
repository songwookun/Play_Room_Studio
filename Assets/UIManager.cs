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

        Debug.Log("���� ���� - Game Over �г� ����");
    }

    public void OnClickRevive()
    {
        var gm = GameManager.Instance;
        gm.health = gm.maxHealth;

        Time.timeScale = 1f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Debug.Log("��Ȱ - ü�� ���� �� ���� �簳");
    }

    public void ShowResult()
    {
        Debug.Log("ShowResult ȣ���");

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
            timeText.text = $"�÷��� �ð�: {minutes:00}:{seconds:00}";

        if (killText != null)
            killText.text = $"���� ����: {gm.Kill}";

        if (coinText != null)
            coinText.text = $"��� ��ȭ: {gm.collectedCoins}";

        PlayerPrefs.SetInt("LastGold", gm.collectedCoins);
        PlayerPrefs.Save();
    }

    public void OnClickDoubleReward()
    {
        GameManager.Instance.collectedCoins *= 2;

        if (coinText != null)
            coinText.text = $"��� ��ȭ: {GameManager.Instance.collectedCoins}";

        PlayerPrefs.SetInt("LastGold", GameManager.Instance.collectedCoins);
        PlayerPrefs.Save();

        Debug.Log("���� 2�� ����");
    }

    public void OnClickExitToStart()
    {
        Time.timeScale = 1f; // Ȥ�� �������� ��츦 ���
        SceneManager.LoadScene("StartScene");
        Debug.Log("������ ��ư Ŭ�� - StartScene���� �̵�");
    }

}
