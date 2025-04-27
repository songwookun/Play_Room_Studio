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
}
