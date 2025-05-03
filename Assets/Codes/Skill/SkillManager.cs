using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public GameObject[] skillPrefabs; // 0: Fire, 1: Ice 등
    public MPManager mpManager;

    private Dictionary<int, SkillData> skillDatas;
    private bool isSpeedBoosted = false;

    private void Start()
    {
        StartCoroutine(SkillDataLoader.LoadSkillDatas(OnSkillDataLoaded));
    }

    void OnSkillDataLoaded()
    {
        Debug.Log("SkillData 로딩 완료");
        skillDatas = SkillDataLoader.skillDatas;
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

        // MP 소모 체크
        if (!mpManager.UseMP(data.cost))
        {
            Debug.Log("MP 부족!");
            return;
        }

        // Boost: 프리팹 없이 작동
        if (data.effectType == "Boost")
        {
            if (!isSpeedBoosted)
                StartCoroutine(ApplySpeedBoost(data));
            return;
        }

        // 나머지 스킬은 프리팹 필요
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

    private IEnumerator ApplySpeedBoost(SkillData data)
    {
        isSpeedBoosted = true;

        var player = GameManager.Instance.player;
        float originalSpeed = player.speed;

        float boostedSpeed = originalSpeed * (1f + data.PlayerSpeed / 100f);
        player.speed = boostedSpeed;

        Debug.Log($"[Boost] {data.PlayerSpeed}% 이속 증가 적용됨 ({data.skillDurat}초)");

        yield return new WaitForSeconds(data.skillDurat);

        player.speed = originalSpeed;
        isSpeedBoosted = false;

        Debug.Log("[Boost] 효과 종료: 이속 원래대로 복귀");
    }

    private Vector3 PlayerPosition()
    {
        return GameManager.Instance.player.transform.position;
    }
}
