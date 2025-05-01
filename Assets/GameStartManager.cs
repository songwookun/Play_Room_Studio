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
    public Button backButton; // 뒤로가기 버튼 변수 추가

    void Start()
    {
        startButton.onClick.AddListener(OnStartGame);
        warriorButton.onClick.AddListener(() => OnClassSelected("Warrior"));
        // mageButton.onClick은 MageUnlockManager에서 처리

        backButton.onClick.AddListener(OnBackToStart); //  이벤트 연결

        classSelectPanel.SetActive(false); // 처음엔 캐릭터 선택 숨김
    }

    void OnStartGame()
    {
        startPanel.SetActive(false);
        classSelectPanel.SetActive(true);
    }

    void OnBackToStart() // 뒤로가기 동작
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
