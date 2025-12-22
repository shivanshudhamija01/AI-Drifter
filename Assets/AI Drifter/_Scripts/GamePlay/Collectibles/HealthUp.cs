using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour, ICollectible
{
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.healthUp;
    }

    public void OnCollected(GameObject collector)
    {

        Debug.Log("Health Collected");
        PlayerHealth health = collector.GetComponent<PlayerHealth>();
        if (health != null)
        {
            PlayerServices.Instance.OnCollectiblePicked.Invoke(Enums.Collectibles.healthUp);
            health.UpdateHealth(100);
        }
        gameObject.SetActive(false);
        // Destroy(gameObject);
    }
}
