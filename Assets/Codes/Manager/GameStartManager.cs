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
    public Button backButton; // 뒤로가기 버튼

    void Awake()
    {
        Debug.Log("GameStartManager Awake() 호출됨");

        startButton.onClick.AddListener(() => {
            Debug.Log("Start 버튼 클릭됨");
            OnStartGame();
        });

        warriorButton.onClick.AddListener(() => {
            Debug.Log("WarriorButton 클릭됨");
            OnClassSelected("Warrior");
        });
    }

    void OnStartGame()
    {
        Debug.Log("Start 버튼 클릭됨");
        startPanel.SetActive(false);
        classSelectPanel.SetActive(true);
    }

    void OnBackToStart()
    {
        Debug.Log("뒤로가기 버튼 클릭됨");
        classSelectPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    void OnClassSelected(string className)
    {
        Debug.Log("클래스 선택됨: " + className);
        PlayerPrefs.SetString("SelectedClass", className);
        SceneManager.LoadScene("WarriorGameScene");
    }
}
