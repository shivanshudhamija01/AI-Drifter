using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour, ICollectible
{
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.speedUp;
    }

    public void OnCollected(GameObject collector)
    {
        Debug.Log("Speed Up Collected");
        Destroy(gameObject);
    }
}
