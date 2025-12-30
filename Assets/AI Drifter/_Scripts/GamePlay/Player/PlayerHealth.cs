using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float health = 0;
    void Awake()
    {
        health = 100;
    }
    void OnEnable()
    {
        UIServices.Instance.updateHealthBar.Invoke(100f);
    }
    public void UpdateHealth(float amount)
    {
        float updateHealth = health + amount;
        updateHealth = Mathf.Clamp(updateHealth, 0, 100);

        health = updateHealth;
        Debug.Log("Updated Player health of player is : " + health);
        UIServices.Instance.updateHealthBar.Invoke(health);
        // Here fires an event that will stop the stop the player and enemy and after some delay a restart panel will be poped out

        if (health <= 0)
        {
            PlayerServices.Instance.OnPlayerDead.Invoke();
        }
    }
}
