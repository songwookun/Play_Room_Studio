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
    public float exp;           // 현재 경험치
    public int level;            // 현재 레벨
    public float Kill;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        health = maxHealth;
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
}
