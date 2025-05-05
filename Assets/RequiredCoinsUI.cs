using UnityEngine;
using UnityEngine.UI;

public class RequiredCoinsUI : MonoBehaviour
{
    [Header("코인 수를 표시할 텍스트")]
    public Text coinText;

    void Start()
    {
        UpdateCoinText(); // 시작 시 UI 텍스트에 저장된 코인 수 표시
    }

    /// <summary>
    /// 저장된 코인 수치를 텍스트로 표시합니다.
    /// </summary>
    public void UpdateCoinText()
    {
        if (CoinManager.Instance != null)
        {
            coinText.text = CoinManager.Instance.GetCoins().ToString();
        }
    }

    /// <summary>
    /// 코인을 1개 추가하고 UI를 갱신합니다.
    /// </summary>
    public void OnCoinCollected()
    {
        CoinManager.Instance.AddCoins(1);
        UpdateCoinText();
    }

    /// <summary>
    /// 코인을 원하는 개수만큼 추가합니다.
    /// </summary>
    public void AddCoins(int amount)
    {
        CoinManager.Instance.AddCoins(amount);
        UpdateCoinText();
    }
}
