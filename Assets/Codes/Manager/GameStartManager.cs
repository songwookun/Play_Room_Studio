using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject classSelectPanel;

    public Button startButton;
    public Button warriorButton;
    public Button mageButton;
    public Button backButton; // �ڷΰ��� ��ư

    void Awake()
    {
        Debug.Log("GameStartManager Awake() ȣ���");

        startButton.onClick.AddListener(() => {
            Debug.Log("Start ��ư Ŭ����");
            OnStartGame();
        });

        warriorButton.onClick.AddListener(() => {
            Debug.Log("WarriorButton Ŭ����");
            OnClassSelected("Warrior");
        });
    }

    void OnStartGame()
    {
        Debug.Log("Start ��ư Ŭ����");
        startPanel.SetActive(false);
        classSelectPanel.SetActive(true);
    }

    void OnBackToStart()
    {
        Debug.Log("�ڷΰ��� ��ư Ŭ����");
        classSelectPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    void OnClassSelected(string className)
    {
        Debug.Log("Ŭ���� ���õ�: " + className);
        PlayerPrefs.SetString("SelectedClass", className);
        SceneManager.LoadScene("WarriorGameScene");
    }
}
