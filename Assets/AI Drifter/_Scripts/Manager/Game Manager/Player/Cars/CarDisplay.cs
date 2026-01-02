using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarDisplay : MonoBehaviour
{
    [Header("Description")]
    [SerializeField] private TextMeshProUGUI carName;

    [SerializeField] private TextMeshProUGUI carPrice;

    [Header("Stats")]
    [SerializeField] private Image speed;
    [SerializeField] private Image acceleration;
    [SerializeField] private Image handling;

    [Header("Car Model")]
    [SerializeField] private GameObject carModel;

    [Header("Purchase")]
    [SerializeField] private GameObject buyButton;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private GameObject selectButton;
    [SerializeField] private GameObject selectedTag;
    [SerializeField] private CarManager carManager;
    [SerializeField] private Button exit;

    private CarSO previousCar;

    void Awake()
    {
        selectButton.gameObject.GetComponent<Button>().onClick.AddListener(UpdateState);
        purchaseButton.onClick.AddListener(UpdateState);
        exit.onClick.AddListener(LoadGamePlayScene);
    }

    private void LoadGamePlayScene()
    {
        // SceneManager.LoadScene("Gameplay");
    }

    public void UpdateCar(CarSO car)
    {
        carName.text = car.CarName;
        carPrice.text = car.CarPrice + " ";

        speed.fillAmount = car.speed / 100f;
        handling.fillAmount = car.handling / 100f;
        acceleration.fillAmount = car.acceleration / 100f;

        if (carModel.transform.childCount > 0)
            Destroy(carModel.transform.GetChild(0).gameObject);

        Instantiate(car.CarModel, carModel.transform.position,
            carModel.transform.rotation, carModel.transform);

        previousCar = car;
        var purchased = carManager.IsPurchased(car);
        var selected = carManager.IsSelected(car);

        carPrice.gameObject.SetActive(!purchased);
        buyButton.SetActive(!purchased);
        selectButton.SetActive(purchased && !selected);
        selectedTag.SetActive(selected);
    }

    void UpdateState()
    {
        var purchased = carManager.IsPurchased(previousCar);
        var selected = carManager.IsSelected(previousCar);

        carPrice.gameObject.SetActive(!purchased);
        buyButton.SetActive(!purchased);
        selectButton.SetActive(purchased && !selected);
        selectedTag.SetActive(selected);
    }
}
