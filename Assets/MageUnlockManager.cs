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
        // Mage ��ư�� Ŭ�� �̺�Ʈ ����
        mageButton.onClick.AddListener(OnMageButtonClick);
        // �ر� Ȯ�� ��ư�� Ŭ�� �̺�Ʈ ����
        confirmUnlockButton.onClick.AddListener(UnlockMage);

        // ���� �� UnlockPanel ����
        unlockPanel.SetActive(false);
    }

    void OnMageButtonClick()
    {
        if (IsUnlocked())
        {
            // �رݵǾ� ������ �ٷ� �� �̵�
            SceneManager.LoadScene("MainScenes");
        }
        else
        {
            // �ر� �ȵǾ����� �ر� �г� ǥ��
            unlockPanel.SetActive(true);
        }
    }

    void UnlockMage()
    {
        int currentCoins = PlayerPrefs.GetInt("AncientCoins", 0);

        if (currentCoins >= requiredCoins)
        {
            // ���� ���� �� �ر� ���� ����
            currentCoins -= requiredCoins;
            PlayerPrefs.SetInt("AncientCoins", currentCoins);
            PlayerPrefs.SetInt(unlockKey, 1);
            PlayerPrefs.Save();

            // �г� ����� �� �̵�
            unlockPanel.SetActive(false);
            SceneManager.LoadScene("MainScenes");
        }
        else
        {
            // ������ �����ϸ� �ֿܼ� �޽��� ���
            Debug.Log("��ȭ�� �����մϴ�!"); // �޽����� UI�� �����ְ� ������ Text ���� �ʿ�
        }
    }

    bool IsUnlocked()
    {
        return PlayerPrefs.GetInt(unlockKey, 0) == 1;
    }
}
