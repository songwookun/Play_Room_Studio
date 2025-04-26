using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManger : MonoBehaviour
{
    // 프리팹 보관 변수
    public GameObject[] prefbs;

    // 풀 담당 리스트 
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

        //선택 한 풀의 놀고 있는 게임 오브젝트 접근

        // 발견하면 select 변수 할당

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // 못 찾으면?
        if (!select)
        {
            //새롭게 생성 하고 select 변수 할당
            select = Instantiate(prefbs[index],transform);
            pools[index].Add(select);
        }

        return select;
    }
}
