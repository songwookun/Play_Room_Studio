using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public List<GameObject> skillPrefabs; // 평타 프리팹들
    public Transform attackPoint;

    private PlayerMove playerMove;
    private SPUM_Prefabs spum;
    private Dictionary<int, NormalAttackData> attackDatas;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        spum = GetComponentInParent<SPUM_Prefabs>();
        attackDatas = NormalAttackDataLoader.LoadData();

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            for (int i = 0; i < skillPrefabs.Count; i++)
            {
                if (!attackDatas.ContainsKey(i) || skillPrefabs[i] == null)
                    continue;

                NormalAttackData current = attackDatas[i];

                if (spum != null)
                    spum.PlayAnimation(PlayerState.ATTACK, 0);

                GameObject slash = Instantiate(skillPrefabs[i], attackPoint.position, Quaternion.identity);
                SkillCollider skillCollider = slash.AddComponent<SkillCollider>();
                skillCollider.damage = current.damge; // CSV 열 이름에 맞춤 (오타 있음 주의)

                if (playerMove.Direction > 0)
                    slash.transform.rotation = Quaternion.Euler(0, 180f, 0);
                else
                    slash.transform.rotation = Quaternion.identity;

                Destroy(slash, current.slashDurat); // CSV 열 이름에 맞춤 (SlashDuration → slashDurat)

                yield return new WaitForSeconds(0.2f);

                if (spum != null)
                {
                    if (playerMove.inputVec != Vector2.zero)
                        spum.PlayAnimation(PlayerState.MOVE, 0);
                    else
                        spum.PlayAnimation(PlayerState.IDLE, 0);
                }

                yield return new WaitForSeconds(current.attackInte - 0.2f); // 쿨타임 (AttackInterval → attackInte)
            }
        }
    }
}
