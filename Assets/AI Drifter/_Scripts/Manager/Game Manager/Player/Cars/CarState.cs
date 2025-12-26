using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarState
{
    public int id;
    public bool purchased;
    public bool selected;
}

[System.Serializable]
public class CarStateList
{
    public List<CarState> cars = new();
}
