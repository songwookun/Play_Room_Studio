using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffect { None, Burn, Slow }

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    public bool isLive = true;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;

    public float rewardExp = 30f;

    public GameObject mpPrefab;
    public float dropChance = 0.3f;

    private Coroutine currentDebuff;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!isLive)
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.linearVelocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!isLive)
            return;
        Vector3 scale = transform.localScale;

        if (target.position.x < rigid.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }

    private void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    public void TakeDamage(float damage)
    {
        if (!isLive) return;

        health -= damage;

        if (health <= 0)
        {
            isLive = false;

            GameManager.Instance.Kill += 1;
            GameManager.Instance.GainExp(rewardExp);

            TryDropMP();

            gameObject.SetActive(false);
        }
    }

    private void TryDropMP()
    {
        if (mpPrefab != null && Random.value < dropChance)
        {
            Instantiate(mpPrefab, transform.position, Quaternion.identity);
        }
    }

    public void ApplyStatus(StatusEffect effect, float duration, float tickDamage = 0f, float speedReduction = 0f)
    {
        if (currentDebuff != null)
            StopCoroutine(currentDebuff);

        currentDebuff = StartCoroutine(HandleStatusEffect(effect, duration, tickDamage, speedReduction));
    }

    private IEnumerator HandleStatusEffect(StatusEffect effect, float duration, float tickDamage, float speedReduction)
    {
        float originalSpeed = speed;

        switch (effect)
        {
            case StatusEffect.Burn:
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    TakeDamage(tickDamage);
                    elapsed += 1f;
                    yield return new WaitForSeconds(1f);
                }
                break;

            case StatusEffect.Slow:
                speed -= speedReduction;
                yield return new WaitForSeconds(duration);
                speed = originalSpeed;
                break;
        }

        currentDebuff = null;
    }
}
