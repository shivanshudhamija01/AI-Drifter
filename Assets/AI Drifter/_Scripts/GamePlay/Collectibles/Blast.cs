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
        if (health != null)
        {
            PlayerServices.Instance.OnCollectiblePicked.Invoke(Enums.Collectibles.blast);
            AudioServices.Instance.PlayAudio.Invoke(Enums.Audios.blast);
            health.UpdateHealth(-50);
        }
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
