using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public List<GameObject> skillPrefabs; // 여러 스킬 프리팹
    public Transform attackPoint;         // 스킬 생성 위치
    public float attackInterval = 1f;      // 스킬 간격 (공통)
    public float skillDuration = 0.5f;     // 스킬 지속시간 (공통)

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

                // 방향 처리
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
