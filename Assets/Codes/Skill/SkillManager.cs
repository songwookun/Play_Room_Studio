using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public GameObject[] skillPrefabs;
    public GameObject fireBoomZoneObject;
    public GameObject fireBoomProjectilePrefab;
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

        if (data.effectType == "Boost" && !isSpeedBoosted)
        {
            StartCoroutine(ApplySpeedBoost(data));
            return;
        }

        if (data.effectType == "Berserk" && !isBerserk)
        {
            StartCoroutine(ApplyBerserk(data));
            return;
        }

        if (skillId == 4)
        {
            StartCoroutine(ActivateFireBoomZoneAndShoot(data));
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

        Debug.Log($"[Berserk] 데미지 +{data.PlayerBerserk}% 적용됨 ({data.skillDurat}초)");
        yield return new WaitForSeconds(data.skillDurat);

        berserkMultiplier = 1.0f;
        isBerserk = false;

        Debug.Log("[Berserk] 종료: 데미지 원래대로 복귀");
    }

    private IEnumerator ActivateFireBoomZoneAndShoot(SkillData data)
    {
        if (fireBoomZoneObject != null)
            fireBoomZoneObject.SetActive(true);

        Vector3[] directions = {
            Vector3.right,
            Vector3.left,
            Vector3.up,
            Vector3.down
        };

        foreach (Vector3 dir in directions)
        {
            GameObject fire = Instantiate(fireBoomProjectilePrefab, PlayerPosition(), Quaternion.identity);

            Rigidbody2D rb = fire.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = dir * 5f;

            // 방향 회전
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            fire.transform.rotation = Quaternion.Euler(0, 0, angle);

            // 방향에 맞춰 Sprite 뒤집기
            SpriteRenderer sr = fire.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                if (dir == Vector3.right)
                {
                    sr.flipX = false;
                    sr.flipY = false;
                }
                else if (dir == Vector3.left)
                {
                    sr.flipX = true;
                    sr.flipY = false;
                }
                else if (dir == Vector3.up)
                {
                    sr.flipX = false;
                    sr.flipY = true;
                }
                else if (dir == Vector3.down)
                {
                    sr.flipX = true;
                    sr.flipY = true;
                }
            }

            // SkillCollider 설정
            SkillCollider collider = fire.GetComponent<SkillCollider>();
            if (collider != null)
            {
                collider.damage = data.damage * berserkMultiplier;
                collider.effectType = data.effectType;
                collider.effectDuration = data.effectDuration;
                collider.tickDamage = data.tickDamage;
                collider.speedReduction = data.speedReduction;
            }

            Destroy(fire, 2f);
        }

        yield return new WaitForSeconds(data.skillDurat);

        if (fireBoomZoneObject != null)
            fireBoomZoneObject.SetActive(false);
    }

    private Vector3 PlayerPosition()
    {
        return GameManager.Instance.player.transform.position;
    }
}
