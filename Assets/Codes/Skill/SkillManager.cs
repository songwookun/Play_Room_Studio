using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public GameObject[] skillPrefabs; // 0: Fire, 1: Ice ��
    public MPManager mpManager;

    private Dictionary<int, SkillData> skillDatas;
    private bool isSpeedBoosted = false;

    private void Start()
    {
        StartCoroutine(SkillDataLoader.LoadSkillDatas(OnSkillDataLoaded));
    }

    void OnSkillDataLoaded()
    {
        Debug.Log("SkillData �ε� �Ϸ�");
        skillDatas = SkillDataLoader.skillDatas;
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

        // MP �Ҹ� üũ
        if (!mpManager.UseMP(data.cost))
        {
            Debug.Log("MP ����!");
            return;
        }

        // Boost: ������ ���� �۵�
        if (data.effectType == "Boost")
        {
            if (!isSpeedBoosted)
                StartCoroutine(ApplySpeedBoost(data));
            return;
        }

        // ������ ��ų�� ������ �ʿ�
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

    private IEnumerator ApplySpeedBoost(SkillData data)
    {
        isSpeedBoosted = true;

        var player = GameManager.Instance.player;
        float originalSpeed = player.speed;

        float boostedSpeed = originalSpeed * (1f + data.PlayerSpeed / 100f);
        player.speed = boostedSpeed;

        Debug.Log($"[Boost] {data.PlayerSpeed}% �̼� ���� ����� ({data.skillDurat}��)");

        yield return new WaitForSeconds(data.skillDurat);

        player.speed = originalSpeed;
        isSpeedBoosted = false;

        Debug.Log("[Boost] ȿ�� ����: �̼� ������� ����");
    }

    private Vector3 PlayerPosition()
    {
        return GameManager.Instance.player.transform.position;
    }
}
