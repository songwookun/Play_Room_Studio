using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public List<GameObject> skillPrefabs; // 평타 프리팹들
    public Transform attackPoint;

    private PlayerMove playerMove;
    private SPUM_Prefabs spum;
    private Dictionary<int, NormalAttackData> attackDatas = new Dictionary<int, NormalAttackData>(); //  초기화

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        spum = GetComponentInParent<SPUM_Prefabs>();

        // 데이터 로드가 완료된 후 코루틴 시작
        StartCoroutine(NormalAttackDataLoader.LoadData(OnNormalAttackDataLoaded));
    }

    void OnNormalAttackDataLoaded()
    {
        Debug.Log("NormalAttackData 로딩 완료");

        attackDatas = NormalAttackDataLoader.normalAttackData; //  데이터 저장

        StartCoroutine(AttackRoutine()); // 데이터가 준비된 후에만 실행
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            for (int i = 0; i < skillPrefabs.Count; i++)
            {
                if (skillPrefabs[i] == null || !attackDatas.ContainsKey(i))
                {
                    Debug.LogWarning($"skillPrefabs[{i}] 또는 공격 데이터 없음");
                    continue;
                }

                NormalAttackData current = attackDatas[i];

                if (spum != null)
                    spum.PlayAnimation(PlayerState.ATTACK, 0);

                GameObject slash = Instantiate(skillPrefabs[i], attackPoint.position, Quaternion.identity);
                SkillCollider skillCollider = slash.AddComponent<SkillCollider>();
                skillCollider.damage = current.damge;

                if (playerMove.Direction > 0)
                    slash.transform.rotation = Quaternion.Euler(0, 180f, 0);
                else
                    slash.transform.rotation = Quaternion.identity;

                Destroy(slash, current.slashDurat);

                yield return new WaitForSeconds(0.2f);

                if (spum != null)
                {
                    if (playerMove.inputVec != Vector2.zero)
                        spum.PlayAnimation(PlayerState.MOVE, 0);
                    else
                        spum.PlayAnimation(PlayerState.IDLE, 0);
                }

                yield return new WaitForSeconds(current.attackInte - 0.2f);
            }
        }
    }
}
