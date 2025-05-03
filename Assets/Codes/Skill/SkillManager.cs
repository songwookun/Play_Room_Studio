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
        Debug.Log("SkillData �ε� �Ϸ�");

        skillDatas = SkillDataLoader.skillDatas; // ��ųʸ� �Ҵ�

        if (skillDatas.TryGetValue(0, out SkillData skill))
        {
            Debug.Log($"ID: 0, ������: {skill.damage}, ���: {skill.cost}");
        }
    }

    public void UseSkill(int skillId)
    {
        if (skillDatas == null)
        {
            Debug.LogError("SkillData�� ���� �ε����� �ʾҽ��ϴ�.");
            return;
        }

        if (!skillDatas.ContainsKey(skillId))
        {
            Debug.LogWarning($"Skill ID {skillId}�� �������� �ʽ��ϴ�.");
            return;
        }

        SkillData data = skillDatas[skillId];

        if (!mpManager.UseMP(data.cost))
        {
            Debug.Log("MP ����!");
            return;
        }

        if (skillPrefabs.Length <= skillId || skillPrefabs[skillId] == null)
        {
            Debug.LogError($"skillPrefabs[{skillId}]�� �������� �ʰų� null�Դϴ�.");
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
