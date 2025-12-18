using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerAttack : MonoBehaviour, ICollectible
{
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.powerAttack;
    }

    public void OnCollected(GameObject collector)
    {
        Debug.Log("Power Attack is performed Collected");
        Destroy(gameObject);
    }
}
