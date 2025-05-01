using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffect { None, Burn, Slow }

[System.Serializable]
public class DropEntry
{
    public GameObject prefab;
    public float dropChance;
}

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    public bool isLive = true;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;

    public float rewardExp = 30f;

    public List<DropEntry> dropTable = new List<DropEntry>(); // 드랍 테이블

    private Coroutine currentDebuff;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        LoadDropTableFromCSV(); // CSV에서 드랍 테이블 불러오기
    }

    private void FixedUpdate()
    {
        if (!isLive) return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.linearVelocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!isLive) return;

        Vector3 scale = transform.localScale;
        scale.x = (target.position.x < rigid.position.x) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    public void TakeDamage(float damage)
    {
        if (!isLive) return;

        health -= damage;

        if (health <= 0)
        {
            isLive = false;

            GameManager.Instance.Kill += 1;
            GameManager.Instance.GainExp(rewardExp);

            TryDropItems();

            gameObject.SetActive(false);
        }
    }

    private void TryDropItems()
    {
        foreach (var entry in dropTable)
        {
            if (entry.prefab != null && Random.value < entry.dropChance)
            {
                Instantiate(entry.prefab, transform.position, Quaternion.identity);
            }
        }
    }

    private void LoadDropTableFromCSV()
    {
        dropTable.Clear();

        TextAsset csvFile = Resources.Load<TextAsset>("DropTable");
        if (csvFile == null)
        {
            Debug.LogError("DropTable.csv 파일을 Resources 폴더에서 찾을 수 없습니다.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] parts = line.Split(',');

            if (parts.Length >= 2)
            {
                string prefabName = parts[0].Trim();
                if (!float.TryParse(parts[1].Trim(), out float chance))
                {
                    Debug.LogWarning($"확률 변환 실패: {line}");
                    continue;
                }

                GameObject prefab = Resources.Load<GameObject>(prefabName);
                if (prefab == null)
                {
                    Debug.LogWarning($"프리팹 로드 실패: {prefabName}");
                    continue;
                }

                dropTable.Add(new DropEntry
                {
                    prefab = prefab,
                    dropChance = chance
                });
            }
        }
    }

    public void ApplyStatus(StatusEffect effect, float duration, float tickDamage = 0f, float speedReduction = 0f)
    {
        if (currentDebuff != null)
            StopCoroutine(currentDebuff);

        currentDebuff = StartCoroutine(HandleStatusEffect(effect, duration, tickDamage, speedReduction));
    }

    private IEnumerator HandleStatusEffect(StatusEffect effect, float duration, float tickDamage, float speedReduction)
    {
        float originalSpeed = speed;

        switch (effect)
        {
            case StatusEffect.Burn:
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    TakeDamage(tickDamage);
                    elapsed += 1f;
                    yield return new WaitForSeconds(1f);
                }
                break;

            case StatusEffect.Slow:
                float slowFactor = Mathf.Clamp01(1f - (speedReduction / 10f));
                speed *= slowFactor;
                yield return new WaitForSeconds(duration);
                speed = originalSpeed;
                break;
        }

        currentDebuff = null;
    }
}
