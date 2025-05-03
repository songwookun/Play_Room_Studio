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
        Debug.Log("SkillData 로딩 완료");
    }

    public void UseSkill(int skillId)
    {
        if (skillDatas == null || !skillDatas.ContainsKey(skillId)) return;

        SkillData data = skillDatas[skillId];

        if (!mpManager.UseMP(data.cost))
        {
            Debug.Log("MP 부족!");
            return;
        }

        // Boost (이속 증가)
        if (data.effectType == "Boost" && !isSpeedBoosted)
        {
            StartCoroutine(ApplySpeedBoost(data));
            return;
        }

        // Berserk (스킬 데미지 증가)
        if (data.effectType == "Berserk" && !isBerserk)
        {
            StartCoroutine(ApplyBerserk(data));
            return;
        }

        // 공격형 스킬 처리
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

        Debug.Log($"[Berserk] 대미지 +{data.PlayerBerserk}% 적용됨 ({data.skillDurat}초)");

        yield return new WaitForSeconds(data.skillDurat);

        berserkMultiplier = 1.0f;
        isBerserk = false;

        Debug.Log("[Berserk] 종료: 대미지 원래대로 복귀");
    }

    private Vector3 PlayerPosition()
    {
        return GameManager.Instance.player.transform.position;
    }
}
