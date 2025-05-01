using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MageUnlockManager : MonoBehaviour
{
    public Button mageButton;
    public GameObject unlockPanel;
    public Button confirmUnlockButton;
    public Button unlockBackButton;

    public GameObject classSelectPanel;
    public GameObject startPanel;

    public Image mageButtonImage;          // MageButton의 이미지 컴포넌트
    public Sprite notAvailableSprite;      // 해금 전 스프라이트
    public Sprite mageSprite;              // 해금 후 스프라이트

    public int requiredCoins = 30;
    private string unlockKey = "MageUnlocked";

    void Start()
    {
        mageButton.onClick.AddListener(OnMageButtonClick);
        confirmUnlockButton.onClick.AddListener(UnlockMage);
        unlockBackButton.onClick.AddListener(CloseUnlockPanel);

        unlockPanel.SetActive(false);

        if (!IsUnlocked())
        {
            mageButtonImage.sprite = notAvailableSprite;
        }
        else
        {
            mageButtonImage.sprite = mageSprite;
        }
    }

    void OnMageButtonClick()
    {
        if (IsUnlocked())
        {
            SceneManager.LoadScene("MainScenes");
        }
        else
        {
            unlockPanel.SetActive(true);
            classSelectPanel.SetActive(false);
        }
    }

    void UnlockMage()
    {
        int currentCoins = PlayerPrefs.GetInt("AncientCoins", 0);

        if (currentCoins >= requiredCoins)
        {
            currentCoins -= requiredCoins;
            PlayerPrefs.SetInt("AncientCoins", currentCoins);
            PlayerPrefs.SetInt(unlockKey, 1);
            PlayerPrefs.Save();

            mageButtonImage.sprite = mageSprite;

            unlockPanel.SetActive(false);
            SceneManager.LoadScene("MainScenes");
        }
        else
        {
            Debug.Log("재화가 부족합니다!");
        }
    }

    void CloseUnlockPanel()
    {
        unlockPanel.SetActive(false);
        classSelectPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    bool IsUnlocked()
    {
        return PlayerPrefs.GetInt(unlockKey, 0) == 1;
    }
}
