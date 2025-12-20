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
   
    [Header("Player")]
    // Player and their different types
    public List<GameObject> Player;

    // Collectible item and their count
    [Header("Collectible Wave Settings")]
    public GameObject CollectiblePrefab;
    public int CollectiblePoolSize;
    public int CollectiblesPerWave;
    public int MinLifeTime;
    public int MaxLiftTime;
    public int RespawnDelay;


    [Header("Power Ups")]
    // Types of power ups and their count
    public List<GameObject> PowerUps;
    public int PowerUpsCount;

    [Header("Timer Setting")]
    // Game Timer
    public int Timer;

}
