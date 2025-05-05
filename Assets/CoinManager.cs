using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    private int requiredCoins = 0;

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 저장된 값 불러오기
            requiredCoins = PlayerPrefs.GetInt("RequiredCoins", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        requiredCoins += amount;
        PlayerPrefs.SetInt("RequiredCoins", requiredCoins); // 저장
        PlayerPrefs.Save();
    }

    public int GetCoins()
    {
        return requiredCoins;
    }

    public void SetCoins(int value)
    {
        requiredCoins = value;
        PlayerPrefs.SetInt("RequiredCoins", requiredCoins);
        PlayerPrefs.Save();
    }

    public void ForceSave()
    {
        PlayerPrefs.SetInt("RequiredCoins", requiredCoins);
        PlayerPrefs.Save();
    }
}
