using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelSO", menuName = "SO/LevelData")]
public class LevelSO : ScriptableObject 
{
    // Enemies and their different types
    // public List<GameObject> Enemies;
    public int smartAI;
    public int madAI;
    public int aggressiveAI;

    public List<GameObject> SmartAI;
    public List<GameObject> MadAI;
    public List<GameObject> AggressiveAI;
    // Player and their different types
    public List<GameObject> Player;

    // Collectible item and their count
    public GameObject Collectible;
    public int collectibleCount;

    // Types of power ups and their count
    public List<CarSO> PowerUps;
    public int PowerUpsCount;

    // Game Timer
    public int Timer;

}
