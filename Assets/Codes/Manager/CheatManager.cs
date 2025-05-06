using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance;

    [Header("치트 설정값")]
    [Tooltip("1이면 무적, 0이면 일반")]
    public int noDamage = 0;

    [Tooltip("시작 시 코인 개수 초기화 (100이면 유지)")]
    public int testCoinAmount = 100;

    [Tooltip("1이면 Mage 해금 상태를 잠금으로 초기화, 0이면 유지")]
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
        // 코인 초기화 조건
        if (testCoinAmount != 100)
        {
            CoinData coinData = new CoinData { totalCoins = testCoinAmount };
            string coinJson = JsonUtility.ToJson(coinData, true);
            File.WriteAllText(coinPath, coinJson);
            Debug.Log($"coin_data.json: {testCoinAmount}개로 초기화됨");
        }
        else
        {
            Debug.Log("coin_data.json: 유지됨 (100은 변경하지 않음)");
        }

        // Mage해금 초기화 조건
        if (resetMageUnlock == 1)
        {
            MageUnlockData unlockData = new MageUnlockData { unlocked = false };
            string unlockJson = JsonUtility.ToJson(unlockData, true);
            File.WriteAllText(unlockPath, unlockJson);
            Debug.Log("mage_unlock.json: 해금 상태 초기화됨 (잠금 상태)");
        }
        else
        {
            Debug.Log(" mage_unlock.json: 변경 없이 유지됨");
        }
    }
}
