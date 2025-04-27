using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    public float speed = 5f;
    public Vector2 inputVec;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        // 본인에서 SpriteRenderer 못 찾으면, 자식에서 찾는다
        spriter = GetComponent<SpriteRenderer>();
        if (spriter == null)
        {
            spriter = GetComponentInChildren<SpriteRenderer>();
        }
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
            // 오른쪽 이동: localScale.x를 -1로 (SPUM 기준)
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (inputVec.x < 0)
        {
            // 왼쪽 이동: localScale.x를 1로
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
