using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int health = 0;
    void Awake()
    {
        health = 100;
    }

    public void UpdateHealth(int amount)
    {
        int updateHealth = health + amount;
        updateHealth = Mathf.Clamp(updateHealth, 0 , 100);

        health = updateHealth;   
        Debug.Log("Updated Player health of player is : " + health);
    }
}
