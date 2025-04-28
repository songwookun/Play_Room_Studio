using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // [�߰�] �� ���͸� ����� �� �� ����ġ��
    public float rewardExp = 30f;

    public GameObject mpPrefab;  // ����� MP ������
    public float dropChance = 0.3f; // 30% Ȯ��

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

            // [�߰�] ���Ͱ� �׾��� �� Kill + 1, ����ġ �߰�
            GameManager.Instance.Kill += 1;
            GameManager.Instance.GainExp(rewardExp);

            TryDropMP();

            gameObject.SetActive(false); // ������ ��Ȱ��ȭ
        }
    }
    private void TryDropMP()
    {
        if (mpPrefab != null)
        {
            if (Random.value < dropChance) // Random.value�� 0~1 ���� ������
            {
                Instantiate(mpPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
