using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectChanger : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] scriptableObjects;
    [SerializeField] private CarManager carManager;

    [Header("Display Scripts")]
    [SerializeField] private CarDisplay carDisplay;
    private int currentMapIndex;

    private void Start()
    {
        ChangeMap(0);
    }

    public void ChangeMap(int _index)
    {
        currentMapIndex += _index;
        if (currentMapIndex < 0) currentMapIndex = scriptableObjects.Length - 1;
        if (currentMapIndex > scriptableObjects.Length - 1) currentMapIndex = 0;

        if (carDisplay != null)
        {
            var carSO = (CarSO)scriptableObjects[currentMapIndex];
            carManager.SetCarSO(carSO);
            carDisplay.UpdateCar((CarSO)scriptableObjects[currentMapIndex]);
        }
    }
}
