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
    public int Direction { get; private set; } = -1; // 기본 방향 왼쪽

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
                Debug.Log($"[체력 감소] 현재 체력: {GameManager.Instance.health}");

                if (GameManager.Instance.health <= 0)
                {
                    GameManager.Instance.health = 0;
                    Debug.Log("[플레이어 사망]");

                    if (spum != null)
                    {
                        spum.PlayAnimation(PlayerState.DEATH, 0);
                    }
                    else
                    {
                        Debug.LogWarning("SPUM_Prefabs 컴포넌트를 찾을 수 없습니다.");
                    }

                    inputVec = Vector2.zero; // 이동 입력 정지
                    StartCoroutine(DeathProcess()); // 죽음 처리 코루틴 시작
                }
            }
        }
    }

    private IEnumerator DeathProcess()
    {
        if (spum != null && spum._anim != null)
        {
            // 죽을 때 애니메이터를 UnscaledTime 모드로 변경
            spum._anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        yield return new WaitForSecondsRealtime(0.8f); // 시간 정지와 상관없이 정확히 0.8초 대기

        //Time.timeScale = 0f; // 게임 멈추기
        Debug.Log("게임 멈춤 (Game Over 상태)");
    }
}
