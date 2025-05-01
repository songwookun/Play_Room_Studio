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
    public Button backButton; // �ڷΰ��� ��ư ���� �߰�

    void Start()
    {
        startButton.onClick.AddListener(OnStartGame);
        warriorButton.onClick.AddListener(() => OnClassSelected("Warrior"));
        // mageButton.onClick�� MageUnlockManager���� ó��

        backButton.onClick.AddListener(OnBackToStart); //  �̺�Ʈ ����

        classSelectPanel.SetActive(false); // ó���� ĳ���� ���� ����
    }

    void OnStartGame()
    {
        startPanel.SetActive(false);
        classSelectPanel.SetActive(true);
    }

    void OnBackToStart() // �ڷΰ��� ����
    {
        classSelectPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    void OnClassSelected(string className)
    {
        PlayerPrefs.SetString("SelectedClass", className);
        SceneManager.LoadScene("MainScenes");
    }
}
