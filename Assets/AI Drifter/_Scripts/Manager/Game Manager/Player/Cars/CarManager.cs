using UnityEngine;

public class CarManager : MonoBehaviour
{
    private CarSO car;
    
    public void PurchaseCar()
    {
        var state = CarSaveSystem.Instance.GetState(car.carID);

        if (state.purchased) return;

        // TODO: deduct coins / currency check
        state.purchased = true;
        CarSaveSystem.Instance.Save();
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