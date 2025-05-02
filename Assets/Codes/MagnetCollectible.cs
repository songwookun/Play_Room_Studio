using UnityEngine;

public class MagnetCollectible : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated) return;
        if (!collision.CompareTag("Player")) return;

        activated = true;

        // �� ���� �±�("item", "MP") ��� ó��
        CollectTaggedItems("item", collision.transform);
        CollectTaggedItems("MP", collision.transform);

        Destroy(gameObject); // ���׳� �������� �����
    }

    private void CollectTaggedItems(string tag, Transform player)
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject item in items)
        {
            if (item == this.gameObject) continue;

            if (item.GetComponent<MagnetTarget>() == null)
            {
                item.AddComponent<MagnetTarget>().Initialize(player);
            }
        }
    }
}
