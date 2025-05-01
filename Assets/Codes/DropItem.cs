using UnityEngine;

public class DropItem : MonoBehaviour
{
    public enum DropItemType
    {
        Coin,
        MP,
        HpSmall,   // 25%
        HpMiddle,  // 50%
        HpBig      // 100%
    }

    public DropItemType itemType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        switch (itemType)
        {
            case DropItemType.Coin:
                GameManager.Instance.GainCoin();
                break;
            case DropItemType.MP:
                GameManager.Instance.GainMP();
                break;
            case DropItemType.HpSmall:
                GameManager.Instance.Heal(0.25f);
                break;
            case DropItemType.HpMiddle:
                GameManager.Instance.Heal(0.5f);
                break;
            case DropItemType.HpBig:
                GameManager.Instance.Heal(1f);
                break;
        }

        Destroy(gameObject);
    }
}
