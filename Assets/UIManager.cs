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
        // �ߺ� ����
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���� ���� �� ȣ��
    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("���� ���� - Game Over �г� ����");
    }

    // ��Ȱ ��ư
    public void OnClickRevive()
    {
        var gm = GameManager.Instance;
        gm.health = gm.maxHealth;

        Time.timeScale = 1f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Debug.Log("��Ȱ - ü�� ���� �� ���� �簳");
    }

    // ��� �г� ǥ��
    public void ShowResult()
    {
        Debug.Log("ShowResult ȣ���");

        if (gameOverPanel != null)
        {
            // GameOverPanel�� �ڽ� ������Ʈ�鸸 ����
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

    // 2�� ���� ��ư
    public void OnClickDoubleReward()
    {
        GameManager.Instance.collectedCoins *= 2;

        if (coinText != null)
            coinText.text = $"��� ��ȭ: {GameManager.Instance.collectedCoins}";

        PlayerPrefs.SetInt("LastGold", GameManager.Instance.collectedCoins);
        PlayerPrefs.Save();

        Debug.Log("���� 2�� ����");
    }

    // ���� ȭ������ �̵�
    public void OnClickGoToStart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }
}
