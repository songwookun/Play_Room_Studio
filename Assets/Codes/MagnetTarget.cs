using UnityEngine;

public class MagnetTarget : MonoBehaviour
{
    private Transform target;
    private float speed = 10f;

    public void Initialize(Transform player)
    {
        target = player;
    }

    void Update()
    {
        if (target == null) return;

        // 플레이어 쪽으로 이동
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // 가까워지면 충돌 유도
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.isTrigger = true;
                Physics2D.IgnoreCollision(col, target.GetComponent<Collider2D>(), false);
            }
        }
    }
}
