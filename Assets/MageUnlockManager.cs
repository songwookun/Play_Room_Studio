using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MageUnlockManager : MonoBehaviour
{
    public Button mageButton;
    public GameObject unlockPanel;
    public Button confirmUnlockButton;
    public int requiredCoins = 30;

    private string unlockKey = "MageUnlocked";

    void Start()
    {
        // Mage 버튼에 클릭 이벤트 연결
        mageButton.onClick.AddListener(OnMageButtonClick);
        // 해금 확인 버튼에 클릭 이벤트 연결
        confirmUnlockButton.onClick.AddListener(UnlockMage);

        // 시작 시 UnlockPanel 숨김
        unlockPanel.SetActive(false);
    }

    void OnMageButtonClick()
    {
        if (IsUnlocked())
        {
            // 해금되어 있으면 바로 씬 이동
            SceneManager.LoadScene("MainScenes");
        }
        else
        {
            // 해금 안되었으면 해금 패널 표시
            unlockPanel.SetActive(true);
        }
    }

    void UnlockMage()
    {
        int currentCoins = PlayerPrefs.GetInt("AncientCoins", 0);

        if (currentCoins >= requiredCoins)
        {
            // 코인 차감 및 해금 상태 저장
            currentCoins -= requiredCoins;
            PlayerPrefs.SetInt("AncientCoins", currentCoins);
            PlayerPrefs.SetInt(unlockKey, 1);
            PlayerPrefs.Save();

            // 패널 숨기고 씬 이동
            unlockPanel.SetActive(false);
            SceneManager.LoadScene("MainScenes");
        }
        else
        {
            // 코인이 부족하면 콘솔에 메시지 출력
            Debug.Log("재화가 부족합니다!"); // 메시지를 UI로 보여주고 싶으면 Text 연결 필요
        }
    }

    bool IsUnlocked()
    {
        return PlayerPrefs.GetInt(unlockKey, 0) == 1;
    }
}
