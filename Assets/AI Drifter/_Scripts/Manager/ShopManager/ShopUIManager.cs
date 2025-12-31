using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    [SerializeField] private Button selectedButton;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI coinCount;
    public static event Action OnButtonClicked;
    public static event Action OnPurchaseButtonClicked;
    public static event Action OnSelectedButtonClicked;

    void Awake()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(ButtonClicked);
        }
        selectedButton.onClick.AddListener(SelectedButtonClicked);
        purchaseButton.onClick.AddListener(PurchaseButtonClicked);
    }

    void OnEnable()
    {
        // Update the coin count 
        OnPurchaseCar();
        CarManager.OnPurchaseSuccessed += OnPurchaseCar;
    }
    void Osable()
    {
        CarManager.OnPurchaseSuccessed += OnPurchaseCar;    
    }
    private void ButtonClicked()
    {
        OnButtonClicked?.Invoke();
    }
    private void PurchaseButtonClicked()
    {
        OnPurchaseButtonClicked?.Invoke();
    }
    private void SelectedButtonClicked()
    {
        OnSelectedButtonClicked?.Invoke();
    }
    void OnPurchaseCar()
    {
        // Modify the coin amount
        int availableCoin = 0;
        if(PlayerPrefs.HasKey("coins"))
        {
            availableCoin = PlayerPrefs.GetInt("coins",0);
        }
        coinCount.text = availableCoin.ToString();
    }
}
