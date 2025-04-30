using UnityEngine;
using UnityEngine.UI;

public class SkillCardClickHandler : MonoBehaviour
{
    public int skillId;
    public SkillManager skillManager;

    private bool isExpanded = false;
    private Vector3 originalScale;
    private Vector3 expandedScale = new Vector3(2f, 2f, 2f);
    private Vector3 originalPosition;
    private Vector3 expandedPosition = new Vector3(0, 0, 0); // 원하는 위치로 수정 가능

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnClickCard()
    {
        if (!isExpanded)
        {
            ExpandCard();
        }
        else
        {
            UseSkill();
            CollapseCard();
        }
    }

    private void ExpandCard()
    {
        isExpanded = true;
        rectTransform.localScale = expandedScale;
        rectTransform.SetAsLastSibling(); // 제일 위에 보이도록
        rectTransform.anchoredPosition = expandedPosition;
    }

    private void CollapseCard()
    {
        isExpanded = false;
        rectTransform.localScale = originalScale;
        rectTransform.anchoredPosition = originalPosition;
    }

    private void UseSkill()
    {
        skillManager.UseSkill(skillId);
    }
}
