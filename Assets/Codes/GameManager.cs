using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameTime;
    public float maxGmaeTime = 2 * 10f;

    public PoolManger pool;
    public PlayerMove player;

    public float exp = 0f;           // ���� ����ġ
    public int level = 0;            // ���� ����
    public float Kill = 0;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGmaeTime)
        {
            gameTime = maxGmaeTime;
        }
    }

    public void GainExp(float amount)
    {
        exp += amount;

        while (exp >= GetNextExp(level))
        {
            exp -= GetNextExp(level);
            level++;
        }
    }

    public float GetNextExp(int currentLevel)
    {
        return 100f * Mathf.Pow(2, currentLevel); 
    }
}
