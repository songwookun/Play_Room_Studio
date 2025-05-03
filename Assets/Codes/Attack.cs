using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public List<GameObject> skillPrefabs; // ��Ÿ �����յ�
    public Transform attackPoint;

    private PlayerMove playerMove;
    private SPUM_Prefabs spum;
    private Dictionary<int, NormalAttackData> attackDatas = new Dictionary<int, NormalAttackData>(); //  �ʱ�ȭ

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        spum = GetComponentInParent<SPUM_Prefabs>();

        // ������ �ε尡 �Ϸ�� �� �ڷ�ƾ ����
        StartCoroutine(NormalAttackDataLoader.LoadData(OnNormalAttackDataLoaded));
    }

    void OnNormalAttackDataLoaded()
    {
        Debug.Log("NormalAttackData �ε� �Ϸ�");

        attackDatas = NormalAttackDataLoader.normalAttackData; //  ������ ����

        StartCoroutine(AttackRoutine()); // �����Ͱ� �غ�� �Ŀ��� ����
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            for (int i = 0; i < skillPrefabs.Count; i++)
            {
                if (skillPrefabs[i] == null || !attackDatas.ContainsKey(i))
                {
                    Debug.LogWarning($"skillPrefabs[{i}] �Ǵ� ���� ������ ����");
                    continue;
                }

                NormalAttackData current = attackDatas[i];

                if (spum != null)
                    spum.PlayAnimation(PlayerState.ATTACK, 0);

                GameObject slash = Instantiate(skillPrefabs[i], attackPoint.position, Quaternion.identity);
                SkillCollider skillCollider = slash.AddComponent<SkillCollider>();
                skillCollider.damage = current.damge;

                if (playerMove.Direction > 0)
                    slash.transform.rotation = Quaternion.Euler(0, 180f, 0);
                else
                    slash.transform.rotation = Quaternion.identity;

                Destroy(slash, current.slashDurat);

                yield return new WaitForSeconds(0.2f);

                if (spum != null)
                {
                    if (playerMove.inputVec != Vector2.zero)
                        spum.PlayAnimation(PlayerState.MOVE, 0);
                    else
                        spum.PlayAnimation(PlayerState.IDLE, 0);
                }

                yield return new WaitForSeconds(current.attackInte - 0.2f);
            }
        }
    }
}
