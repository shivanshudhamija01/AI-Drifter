using System.IO;
using UnityEngine;

public class CarSaveSystem : MonoBehaviour
{
    public static CarSaveSystem Instance;

    public CarStateList state = new();
    private string filePath;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        filePath = Path.Combine(Application.persistentDataPath, "CarPurchaseds.json");
        Load();
    }

    public void Load()
    {
        if (!File.Exists(filePath))
        {
            // First launch → create default data
            state = new CarStateList();
            Save();
            return;
        }

        string json = File.ReadAllText(filePath);
        state = JsonUtility.FromJson<CarStateList>(json);
        Debug.Log("File path is : " + filePath);
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(state, true);
        File.WriteAllText(filePath, json);
    }

    public CarState GetState(int id)
    {
        var car = state.cars.Find(c => c.id == id);

        if (car == null)
        {
            car = new CarState { id = id, purchased = (id == 1), selected = (id == 1) };
            state.cars.Add(car);
            Save();
        }

        return car;
    }

}
