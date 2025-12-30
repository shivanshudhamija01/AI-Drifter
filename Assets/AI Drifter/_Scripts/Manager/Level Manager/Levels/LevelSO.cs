using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelSO", menuName = "SO/LevelData")]
public class LevelSO : ScriptableObject
{

    // Enemies and their different types
    // public List<GameObject> Enemies;
    [Header("Enemy")]
    [Header("Particular Enemy Type Count")]
    public int smartAI;
    public int madAI;
    public int aggressiveAI;

    [Header("Enemy types and their prefab")]
    public List<GameObject> SmartAI;
    public List<GameObject> MadAI;
    public List<GameObject> AggressiveAI;

    [Header("Reward")]
    public int RewardValue;

    // Collectible item and their count
    [Header("Collectible")]
    public GameObject CollectiblePrefab;
    public int CollectibleCount;


    [Header("Power Ups")]

    [Header("PowerUps Wave Settings")]
    // Types of power ups and their count
    public List<GameObject> PowerUps;
    public int PowerUpPoolSize;
    public int PowerUpPerWave;
    public int MinLifeTime;
    public int MaxLiftTime;
    public int RespawnDelay;


}
