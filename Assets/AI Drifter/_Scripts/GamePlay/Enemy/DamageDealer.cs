using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour, IDamage
{
    [SerializeField] private int damageAmount = -1;
    public int GetDamage()
    {
        return damageAmount;
    }
}
