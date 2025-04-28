using System.Collections.Generic;
using UnityEngine;

public class MPManager : MonoBehaviour
{
    public List<GameObject> mpImages = new List<GameObject>();
    private int currentMP = 0;

    private void Start()
    {
        // ������ �� ��� MP �̹����� ��Ȱ��ȭ
        foreach (var img in mpImages)
        {
            img.SetActive(false);
        }
    }

    public void AddMP()
    {
        if (currentMP < mpImages.Count)
        {
            mpImages[currentMP].SetActive(true);
            currentMP++;
        }
    }
}
