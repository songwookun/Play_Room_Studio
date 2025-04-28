using UnityEngine;

public class MPCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어에 붙어있는 MPManager 찾아서 AddMP 호출
            MPManager manager = FindFirstObjectByType<MPManager>();
            if (manager != null)
            {
                manager.AddMP();
            }
            Destroy(gameObject); // MP 아이템 삭제
        }
    }
}
