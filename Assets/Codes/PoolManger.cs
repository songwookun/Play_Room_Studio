using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManger : MonoBehaviour
{
    // ������ ���� ����
    public GameObject[] prefbs;

    // Ǯ ��� ����Ʈ 
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefbs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }
    public GameObject Get(int index)
    {
        GameObject select = null;

        //���� �� Ǯ�� ��� �ִ� ���� ������Ʈ ����

        // �߰��ϸ� select ���� �Ҵ�

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // �� ã����?
        if (!select)
        {
            //���Ӱ� ���� �ϰ� select ���� �Ҵ�
            select = Instantiate(prefbs[index],transform);
            pools[index].Add(select);
        }

        return select;
    }
}
