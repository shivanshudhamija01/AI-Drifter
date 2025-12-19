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
        Debug.Log("Fire's an event to the level manager that will multiply the number of enemies in the scene or just , duplicate the cars");
        Destroy(gameObject);
    }
}
