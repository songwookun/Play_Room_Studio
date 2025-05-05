using UnityEngine;
using UnityEngine.UI;

public class RequiredCoinsUI : MonoBehaviour
{
    [Header("���� ���� ǥ���� �ؽ�Ʈ")]
    public Text coinText;

    void Start()
    {
        UpdateCoinText(); // ���� �� UI �ؽ�Ʈ�� ����� ���� �� ǥ��
    }

    /// <summary>
    /// ����� ���� ��ġ�� �ؽ�Ʈ�� ǥ���մϴ�.
    /// </summary>
    public void UpdateCoinText()
    {
        if (CoinManager.Instance != null)
        {
            coinText.text = CoinManager.Instance.GetCoins().ToString();
        }
    }

    /// <summary>
    /// ������ 1�� �߰��ϰ� UI�� �����մϴ�.
    /// </summary>
    public void OnCoinCollected()
    {
        CoinManager.Instance.AddCoins(1);
        UpdateCoinText();
    }

    /// <summary>
    /// ������ ���ϴ� ������ŭ �߰��մϴ�.
    /// </summary>
    public void AddCoins(int amount)
    {
        CoinManager.Instance.AddCoins(amount);
        UpdateCoinText();
    }
}
