using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public List<GameObject> skillPrefabs; // ���� ��ų ������
    public Transform attackPoint;         // ��ų ���� ��ġ
    public float attackInterval = 1f;      // ��ų ���� (����)
    public float skillDuration = 0.5f;     // ��ų ���ӽð� (����)

    private PlayerMove playerMove;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (skillPrefabs.Count == 0)
            {
                yield return null;
                continue;
            }

            foreach (var prefab in skillPrefabs)
            {
                if (prefab == null)
                    continue;

                GameObject slash = Instantiate(prefab, attackPoint.position, Quaternion.identity);

                // ���� ó��
                if (playerMove.Direction > 0)
                    slash.transform.rotation = Quaternion.Euler(0, 180f, 0);
                else
                    slash.transform.rotation = Quaternion.identity;

                Destroy(slash, skillDuration);

                yield return new WaitForSeconds(attackInterval);
            }
        }
    }
}
