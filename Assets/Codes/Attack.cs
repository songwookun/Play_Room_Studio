using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public List<GameObject> skillPrefabs;
    public Transform attackPoint;

    private PlayerMove playerMove;
    private SPUM_Prefabs spum;
    private Dictionary<int, SkillData> skillDatas;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        spum = GetComponentInParent<SPUM_Prefabs>(); 
        skillDatas = SkillDataLoader.LoadSkillDatas();

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            for (int i = 0; i < skillPrefabs.Count; i++)
            {
                if (!skillDatas.ContainsKey(i) || skillPrefabs[i] == null)
                    continue;

                SkillData currentSkillData = skillDatas[i];

                if (spum != null)
                {
                    spum.PlayAnimation(PlayerState.ATTACK, 0);
                }


                GameObject slash = Instantiate(skillPrefabs[i], attackPoint.position, Quaternion.identity);

                SkillCollider skillCollider = slash.AddComponent<SkillCollider>();
                skillCollider.damage = currentSkillData.damage;

                if (playerMove.Direction > 0)
                    slash.transform.rotation = Quaternion.Euler(0, 180f, 0);
                else
                    slash.transform.rotation = Quaternion.identity;

                Destroy(slash, currentSkillData.skillDuration);


                yield return new WaitForSeconds(0.2f);

                if (spum != null)
                {
                    if (playerMove.inputVec != Vector2.zero)
                    {
                        spum.PlayAnimation(PlayerState.MOVE, 0);
                    }
                    else
                    {
                        spum.PlayAnimation(PlayerState.IDLE, 0);
                    }
                }

                // 스킬 쿨타임 기다리기
                yield return new WaitForSeconds(currentSkillData.attackInterval - 0.2f);
            }
        }
    }

}
