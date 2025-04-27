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

        // ���ο��� SpriteRenderer �� ã����, �ڽĿ��� ã�´�
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
            // ������ �̵�: localScale.x�� -1�� (SPUM ����)
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (inputVec.x < 0)
        {
            // ���� �̵�: localScale.x�� 1��
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
