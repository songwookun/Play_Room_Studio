using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public List<GameObject> skillPrefabs;
    public Transform attackPoint;

    private PlayerMove playerMove;
    private Dictionary<int, SkillData> skillDatas;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
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

                GameObject slash = Instantiate(skillPrefabs[i], attackPoint.position, Quaternion.identity);

                SkillCollider skillCollider = slash.AddComponent<SkillCollider>();
                skillCollider.damage = currentSkillData.damage;


                if (playerMove.Direction > 0)
                    slash.transform.rotation = Quaternion.Euler(0, 180f, 0);
                else
                    slash.transform.rotation = Quaternion.identity;

                Destroy(slash, currentSkillData.skillDuration);

                yield return new WaitForSeconds(currentSkillData.attackInterval);
            }
        }
    }
}
