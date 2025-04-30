using UnityEngine;

public class SkillCollider : MonoBehaviour
{
    public float damage;
    public GameObject hitEffectPrefab;

    // CSV���� ���޹��� �߰� ������
    public string effectType;
    public float effectDuration;
    public float tickDamage;
    public float speedReduction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                if (enemy.isLive)
                {
                    // CSV�� ���� �����̻� ����
                    if (effectType == "Burn")
                    {
                        enemy.ApplyStatus(StatusEffect.Burn, effectDuration, tickDamage);
                    }
                    else if (effectType == "Slow")
                    {
                        enemy.ApplyStatus(StatusEffect.Slow, effectDuration, 0f, speedReduction);
                    }
                }

                if (hitEffectPrefab != null)
                {
                    Vector3 hitPosition = collision.bounds.center;
                    GameObject effect = Instantiate(hitEffectPrefab, hitPosition, Quaternion.identity);
                    Destroy(effect, 0.1f);
                }
            }
        }
    }
}
