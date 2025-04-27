using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.player != null)
        {
            rect.position = Camera.main.WorldToScreenPoint(GameManager.Instance.player.transform.position);
        }
    }
}
