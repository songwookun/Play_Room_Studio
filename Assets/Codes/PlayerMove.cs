using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    public float speed = 5f;
    public Vector2 inputVec;
    public int Direction { get; private set; } = -1; // �⺻ ���� ����

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        if (spriter == null)
            spriter = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        // ������ �� ���� ���� ���� ����
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
    }
}
