using System;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    private CarSO car;
    public static event Action OnPurchaseSuccessed;
    public static event Action OnPurchaseFail;
    public void PurchaseCar()
    {
        var state = CarSaveSystem.Instance.GetState(car.carID);

        if (state.purchased) return;

        // TODO: deduct coins / currency check
        int availableCoin = 0;
        if(PlayerPrefs.HasKey("coins"))
        {
            availableCoin = PlayerPrefs.GetInt("coins",0);
        }

        int priceOfCar = car.CarPrice;
        //
        if(availableCoin>= priceOfCar)
        {
            availableCoin-=priceOfCar;
            PlayerPrefs.SetInt("coins",availableCoin);
            PlayerPrefs.Save();
            state.purchased = true;
            CarSaveSystem.Instance.Save();
            OnPurchaseSuccessed?.Invoke();
        }
        else
        {
            OnPurchaseFail?.Invoke();
            return;
        } 
    }

    public void SelectCar()
    {
        foreach (var c in CarSaveSystem.Instance.state.cars)
            c.selected = false;

        var state = CarSaveSystem.Instance.GetState(car.carID);
        state.selected = true;
        
        CarSaveSystem.Instance.Save();
        
        CarSaveSystem.Instance.SaveSelectedCarID(car.carID);
        
        Debug.Log($"Selected car: {car.CarName} (ID: {car.carID})");
    }
    
    public void SetCarSO(CarSO carSO)
    {
        car = carSO;
    }
    
    public bool IsPurchased(CarSO car)
        => CarSaveSystem.Instance.GetState(car.carID).purchased;

    public bool IsSelected(CarSO car)
        => CarSaveSystem.Instance.GetState(car.carID).selected;
}