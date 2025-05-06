using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance;

    [Header("ġƮ ������")]
    [Tooltip("1�̸� ����, 0�̸� �Ϲ�")]
    public int noDamage = 0;

    [Tooltip("���� �� ���� ���� �ʱ�ȭ (100�̸� ����)")]
    public int testCoinAmount = 100;

    [Tooltip("1�̸� Mage �ر� ���¸� ������� �ʱ�ȭ, 0�̸� ����")]
    public int resetMageUnlock = 0;

    private string coinPath;
    private string unlockPath;

    public bool IsNoDamageActive()
    {
        return noDamage == 1;
    }

    private void Awake()
    {
        Instance = this;

        coinPath = Path.Combine(Application.persistentDataPath, "coin_data.json");
        unlockPath = Path.Combine(Application.persistentDataPath, "mage_unlock.json");
    }

    private void Start()
    {
        // ���� �ʱ�ȭ ����
        if (testCoinAmount != 100)
        {
            CoinData coinData = new CoinData { totalCoins = testCoinAmount };
            string coinJson = JsonUtility.ToJson(coinData, true);
            File.WriteAllText(coinPath, coinJson);
            Debug.Log($"coin_data.json: {testCoinAmount}���� �ʱ�ȭ��");
        }
        else
        {
            Debug.Log("coin_data.json: ������ (100�� �������� ����)");
        }

        // Mage�ر� �ʱ�ȭ ����
        if (resetMageUnlock == 1)
        {
            MageUnlockData unlockData = new MageUnlockData { unlocked = false };
            string unlockJson = JsonUtility.ToJson(unlockData, true);
            File.WriteAllText(unlockPath, unlockJson);
            Debug.Log("mage_unlock.json: �ر� ���� �ʱ�ȭ�� (��� ����)");
        }
        else
        {
            Debug.Log(" mage_unlock.json: ���� ���� ������");
        }
    }
}
