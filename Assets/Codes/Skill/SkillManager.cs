using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public GameObject[] skillPrefabs; // 0: Fire, 1: Ice
    public MPManager mpManager;

    private Dictionary<int, SkillData> skillDatas;

    private void Start()
    {
        StartCoroutine(SkillDataLoader.LoadSkillDatas(OnSkillDataLoaded));
    }

    void OnSkillDataLoaded()
    {
        Debug.Log("SkillData 로딩 완료");

        skillDatas = SkillDataLoader.skillDatas; // 딕셔너리 할당

        if (skillDatas.TryGetValue(0, out SkillData skill))
        {
            Debug.Log($"ID: 0, 데미지: {skill.damage}, 비용: {skill.cost}");
        }
    }

    public void UseSkill(int skillId)
    {
        if (skillDatas == null)
        {
            Debug.LogError("SkillData가 아직 로딩되지 않았습니다.");
            return;
        }

        if (!skillDatas.ContainsKey(skillId))
        {
            Debug.LogWarning($"Skill ID {skillId}가 존재하지 않습니다.");
            return;
        }

        SkillData data = skillDatas[skillId];

        if (!mpManager.UseMP(data.cost))
        {
            Debug.Log("MP 부족!");
            return;
        }

        if (skillPrefabs.Length <= skillId || skillPrefabs[skillId] == null)
        {
            Debug.LogError($"skillPrefabs[{skillId}]가 존재하지 않거나 null입니다.");
            return;
        }

        GameObject skill = Instantiate(skillPrefabs[skillId], PlayerPosition(), Quaternion.identity);

        if (GameManager.Instance.player.Direction > 0)
            skill.transform.rotation = Quaternion.Euler(0, 180f, 0);
        else
            skill.transform.rotation = Quaternion.identity;

        SkillCollider collider = skill.GetComponent<SkillCollider>();
        if (collider != null)
        {
            collider.damage = data.damage;
            collider.effectType = data.effectType;
            collider.effectDuration = data.effectDuration;
            collider.tickDamage = data.tickDamage;
            collider.speedReduction = data.speedReduction;
        }

        Destroy(skill, data.skillDurat);
    }

    private Vector3 PlayerPosition()
    {
        return GameManager.Instance.player.transform.position;
    }
}
