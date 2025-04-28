using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameTime;
    public float maxGmaeTime;

    public PoolManger pool;
    public PlayerMove player;
    public int health;
    public int maxHealth = 100;
    public float exp;
    public int level;
    public float Kill;

    public int mp;             // 현재 MP
    public int maxMp = 10;      // 최대 MP

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        health = maxHealth;
        mp = 0;                 // MP도 초기화
    }

    void Update()
    {
        gameTime += Time.deltaTime;
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

    public void GainMp(int amount)
    {
        mp += amount;
        if (mp > maxMp)
            mp = maxMp;
    }

    public void UseMp(int amount)
    {
        mp -= amount;
        if (mp < 0)
            mp = 0;
    }
}
