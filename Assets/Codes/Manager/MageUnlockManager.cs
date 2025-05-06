using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class MageUnlockData
{
    public bool unlocked = false;
}


public class MageUnlockManager : MonoBehaviour
{
    public Button mageButton;
    public GameObject unlockPanel;
    public Button confirmUnlockButton;
    public Button unlockBackButton;

    public GameObject classSelectPanel;
    public GameObject startPanel;

    public Image mageButtonImage;
    public Sprite notAvailableSprite;
    public Sprite mageSprite;

    public int requiredCoins = 30;

    private string coinPath;
    private string unlockPath;

    private void Start()
    {
        mageButton.onClick.AddListener(OnMageButtonClick);
        confirmUnlockButton.onClick.AddListener(UnlockMage);
        unlockBackButton.onClick.AddListener(CloseUnlockPanel);

        coinPath = Path.Combine(Application.persistentDataPath, "coin_data.json");
        unlockPath = Path.Combine(Application.persistentDataPath, "mage_unlock.json");

        unlockPanel.SetActive(false);

        if (!IsUnlocked())
            mageButtonImage.sprite = notAvailableSprite;
        else
            mageButtonImage.sprite = mageSprite;
    }
    public void TestClick()
    {
        Debug.Log("Test 버튼 눌림!");
    }

    void OnMageButtonClick()
    {
        if (IsUnlocked())
        {
            SceneManager.LoadScene("MageGameScene");
        }
        else
        {
            unlockPanel.SetActive(true);
            classSelectPanel.SetActive(false);
        }
    }

    public void UnlockMage()
    {
        int currentCoins = LoadCoins();  // coin_data.json에서 현재 코인 로드

        if (currentCoins >= requiredCoins)
        {
            currentCoins -= requiredCoins;
            SaveCoins(currentCoins);               // 차감 후 저장
            SaveUnlockState(true);                 // 해금 상태 저장
            mageButtonImage.sprite = mageSprite;   // 이미지 변경
            unlockPanel.SetActive(false);
            SceneManager.LoadScene("MageGameScene"); // 씬 이동
        }
        else
        {
            Debug.Log("코인이 부족합니다!");
        }
    }

    int LoadCoins()
    {
        if (File.Exists(coinPath))
        {
            string json = File.ReadAllText(coinPath);
            CoinData data = JsonUtility.FromJson<CoinData>(json);
            return data.totalCoins;
        }
        return 0;
    }

    void SaveCoins(int amount)
    {
        CoinData data = new CoinData { totalCoins = amount };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(coinPath, json);
    }

    bool IsUnlocked()
    {
        if (File.Exists(unlockPath))
        {
            string json = File.ReadAllText(unlockPath);
            MageUnlockData data = JsonUtility.FromJson<MageUnlockData>(json);
            return data.unlocked;
        }
        return false;
    }

    void SaveUnlockState(bool state)
    {
        MageUnlockData data = new MageUnlockData { unlocked = state };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(unlockPath, json);
    }

    void CloseUnlockPanel()
    {
        unlockPanel.SetActive(false);
        classSelectPanel.SetActive(false);
        startPanel.SetActive(true);
    }
}
