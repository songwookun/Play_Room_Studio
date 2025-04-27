using UnityEngine;

public class SkillCollider : MonoBehaviour
{
    public float damage;
    public GameObject hitEffectPrefab; // �߰�: ����Ʈ ������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                if (hitEffectPrefab != null)
                {
                    // Collider �߽ɿ� ����Ʈ ����
                    Vector3 hitPosition = collision.bounds.center;
                    GameObject effect = Instantiate(hitEffectPrefab, hitPosition, Quaternion.identity);
                    Destroy(effect, 0.1f);
                }
            }
        }
    }
}