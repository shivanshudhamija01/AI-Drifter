using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemical : MonoBehaviour, ICollectible
{
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.chemical;
    }

    public void OnCollected(GameObject collector)
    {
        Debug.Log("Chemical Collected");
        Destroy(gameObject);
    }
}
