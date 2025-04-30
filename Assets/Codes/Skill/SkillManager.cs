using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public GameObject[] skillPrefabs; // 0: Fire, 1: Ice
    public MPManager mpManager;

    private Dictionary<int, SkillData> skillDatas;

    private void Start()
    {
        skillDatas = SkillDataLoader.LoadSkillDatas();
    }

    public void UseSkill(int skillId)
    {
        SkillData data = skillDatas[skillId];
        if (!mpManager.UseMP(data.cost)) 
        {
            Debug.Log("MP 부족!");
            return;
        }


        if (!skillDatas.ContainsKey(skillId)) return;

        GameObject skill = Instantiate(skillPrefabs[skillId], PlayerPosition(), Quaternion.identity);

        //플레이어 방향에 따라 Y축 반전 처리
        if (GameManager.Instance.player.Direction > 0)
            skill.transform.rotation = Quaternion.Euler(0, 180f, 0); // 오른쪽 바라볼 때 반전
        else
            skill.transform.rotation = Quaternion.identity; // 왼쪽은 그대로

        // skillCollider 세팅
        SkillCollider collider = skill.GetComponent<SkillCollider>();
        if (collider != null)
        {
            SkillData sd = skillDatas[skillId];
            collider.damage = sd.damage;
            collider.effectType = sd.effectType;
            collider.effectDuration = sd.effectDuration;
            collider.tickDamage = sd.tickDamage;
            collider.speedReduction = sd.speedReduction;
        }

        //일정 시간 뒤 자동 제거
        Destroy(skill, skillDatas[skillId].skillDurat);
    }

    private Vector3 PlayerPosition()
    {
        return GameManager.Instance.player.transform.position;
    }
}
