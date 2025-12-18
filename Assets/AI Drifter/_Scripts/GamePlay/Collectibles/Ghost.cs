using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour, ICollectible
{
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.ghost;
    }

    public void OnCollected(GameObject collector)
    {
        Debug.Log("Ghost Collected");
        Destroy(gameObject);
    }
}
