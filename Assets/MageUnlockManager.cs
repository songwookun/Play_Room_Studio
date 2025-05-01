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

    public Image mageButtonImage;          // MageButton�� �̹��� ������Ʈ
    public Sprite notAvailableSprite;      // �ر� �� ��������Ʈ
    public Sprite mageSprite;              // �ر� �� ��������Ʈ

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
            Debug.Log("��ȭ�� �����մϴ�!");
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
