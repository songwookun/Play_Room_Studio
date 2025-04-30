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

    public bool UseMP(int amount)
    {
        if (currentMP < amount)
            return false;

        for (int i = 0; i < amount; i++)
        {
            currentMP--;
            mpImages[currentMP].SetActive(false);
        }

        return true;
    }

}
