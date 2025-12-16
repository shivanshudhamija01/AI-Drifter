using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CarState
{
    Locked,
    Unlocked
}
[CreateAssetMenu(fileName = "Car", menuName = "SO/CarData")]
public class CarSO : ScriptableObject
{
    // Car Name 
    public string Name;
    // Car Model 
    public GameObject Prefab;
    // Car Cost 
    public int Coins;
    #region  CarFeatures
    // Speed
    public float Speed;
    // Drift Amout
    public float Drift;
    // Acceleration 
    public float Acceleration;
    // Traction
    public float Traction;
    // Tilt value
    public float tiltValue;
    #endregion
    // Locked or Unlocked state
    public CarState carState;
    // Selected or not selected
    public bool isSelected;
}
