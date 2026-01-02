using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour
{
    [SerializeField] private GameObject gamePlayScene;
    [SerializeField] private GameObject shopScene;
    [SerializeField] private Material shopMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button exitButton;


    void Awake()
    {
        shopButton.onClick.AddListener(EnableShop);
        exitButton.onClick.AddListener(EnableGamePlay);
        gamePlayScene.SetActive(true);
        shopScene.SetActive(false);
    }

    void EnableGamePlay()
    {
        gamePlayScene.SetActive(true);
        shopScene.SetActive(false);
        RenderSettings.skybox = defaultMaterial;
    }
    void EnableShop()
    {
        gamePlayScene.SetActive(false);
        shopScene.SetActive(true);
        RenderSettings.skybox = shopMaterial;
    }
}
