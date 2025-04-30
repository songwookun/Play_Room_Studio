using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    public float speed = 5f;
    public Vector2 inputVec;
    public int Direction { get; private set; } = -1; // �⺻ ���� ����

    private SPUM_Prefabs spum;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        if (spriter == null)
            spriter = GetComponentInChildren<SpriteRenderer>();

        spum = GetComponentInParent<SPUM_Prefabs>();
    }

    private void Start()
    {
        transform.localScale = new Vector3(1, 1, 1);
        Direction = -1;
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        if (inputVec.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            Direction = 1;
        }
        else if (inputVec.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            Direction = -1;
        }

        if (spum != null)
        {
            if (inputVec != Vector2.zero)
            {
                spum.PlayAnimation(PlayerState.MOVE, 0);
            }
            else
            {
                spum.PlayAnimation(PlayerState.IDLE, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.GetComponent<Enemy>() != null)
        {
            float distance = Vector2.Distance(transform.position, collision.transform.position);
            if (distance < 1.0f)
            {
                GameManager.Instance.health -= 10;
                Debug.Log($"[ü�� ����] ���� ü��: {GameManager.Instance.health}");

                if (GameManager.Instance.health <= 0)
                {
                    GameManager.Instance.health = 0;
                    Debug.Log("[�÷��̾� ���]");

                    if (spum != null)
                    {
                        spum.PlayAnimation(PlayerState.DEATH, 0);
                    }
                    else
                    {
                        Debug.LogWarning("SPUM_Prefabs ������Ʈ�� ã�� �� �����ϴ�.");
                    }

                    inputVec = Vector2.zero; // �̵� �Է� ����
                    StartCoroutine(DeathProcess()); // ���� ó�� �ڷ�ƾ ����
                }
            }
        }
    }

    private IEnumerator DeathProcess()
    {
        if (spum != null && spum._anim != null)
        {
            // ���� �� �ִϸ����͸� UnscaledTime ���� ����
            spum._anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        yield return new WaitForSecondsRealtime(0.8f); // �ð� ������ ������� ��Ȯ�� 0.8�� ���

        //Time.timeScale = 0f; // ���� ���߱�
        Debug.Log("���� ���� (Game Over ����)");
    }
}
