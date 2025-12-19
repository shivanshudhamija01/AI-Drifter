using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour, ICollectible
{
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.blast;
    }

    public void OnCollected(GameObject collector)
    {
        Debug.Log("Blast Collected");
        PlayerHealth health = collector.GetComponent<PlayerHealth>();
        if(health != null)
        {
            health.UpdateHealth(-50);
        }
        Destroy(gameObject);
    }
}
