using UnityEngine;

public class MPCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �÷��̾ �پ��ִ� MPManager ã�Ƽ� AddMP ȣ��
            MPManager manager = FindFirstObjectByType<MPManager>();
            if (manager != null)
            {
                manager.AddMP();
            }
            Destroy(gameObject); // MP ������ ����
        }
    }
}
