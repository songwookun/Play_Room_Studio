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

    void Start()
    {
        startButton.onClick.AddListener(OnStartGame);
        warriorButton.onClick.AddListener(() => OnClassSelected("Warrior"));
        //mageButton.onClick.AddListener(() => OnClassSelected("Mage"));

        classSelectPanel.SetActive(false); // 처음엔 숨기기
    }

    void OnStartGame()
    {
        startPanel.SetActive(false);
        classSelectPanel.SetActive(true);
    }

    void OnClassSelected(string className)
    {
        PlayerPrefs.SetString("SelectedClass", className);
        SceneManager.LoadScene("MainScenes");
    }
}
