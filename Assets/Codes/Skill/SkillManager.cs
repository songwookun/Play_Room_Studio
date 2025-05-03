using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public GameObject[] skillPrefabs;
    public MPManager mpManager;

    private Dictionary<int, SkillData> skillDatas;
    private bool isSpeedBoosted = false;
    private bool isBerserk = false;
    private float berserkMultiplier = 1.0f;

    private void Start()
    {
        StartCoroutine(SkillDataLoader.LoadSkillDatas(OnSkillDataLoaded));
    }

    void OnSkillDataLoaded()
    {
        skillDatas = SkillDataLoader.skillDatas;
        Debug.Log("SkillData �ε� �Ϸ�");
    }

    public void UseSkill(int skillId)
    {
        if (skillDatas == null || !skillDatas.ContainsKey(skillId)) return;

        SkillData data = skillDatas[skillId];

        if (!mpManager.UseMP(data.cost))
        {
            Debug.Log("MP ����!");
            return;
        }

        // Boost (�̼� ����)
        if (data.effectType == "Boost" && !isSpeedBoosted)
        {
            StartCoroutine(ApplySpeedBoost(data));
            return;
        }

        // Berserk (��ų ������ ����)
        if (data.effectType == "Berserk" && !isBerserk)
        {
            StartCoroutine(ApplyBerserk(data));
            return;
        }

        // ������ ��ų ó��
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
            collider.damage = data.damage * berserkMultiplier;
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

        player.speed = originalSpeed * (1f + data.PlayerSpeed / 100f);
        yield return new WaitForSeconds(data.skillDurat);

        player.speed = originalSpeed;
        isSpeedBoosted = false;
    }

    private IEnumerator ApplyBerserk(SkillData data)
    {
        isBerserk = true;
        berserkMultiplier = 1f + data.PlayerBerserk / 100f;

        Debug.Log($"[Berserk] ����� +{data.PlayerBerserk}% ����� ({data.skillDurat}��)");

        yield return new WaitForSeconds(data.skillDurat);

        berserkMultiplier = 1.0f;
        isBerserk = false;

        Debug.Log("[Berserk] ����: ����� ������� ����");
    }

    private Vector3 PlayerPosition()
    {
        return GameManager.Instance.player.transform.position;
    }
}
