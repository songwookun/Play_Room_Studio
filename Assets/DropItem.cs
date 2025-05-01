using UnityEngine;

public class DropItem : MonoBehaviour
{
    public enum DropItemType
    {
        Coin,
        MP
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
        }

        Destroy(gameObject);
    }
}
