using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.coin;
    }

    public void OnCollected(GameObject collector)
    {
        Debug.Log("Coin Collected");
        AudioServices.Instance.PlayAudio.Invoke(Enums.Audios.coinPickedUp);
        // This event will be listened by the arrow, so that they after coin collection, they will not point toward the that coin 
        PlayerServices.Instance.OnCoinPickedUp.Invoke(this.gameObject);
        GameManager.Instance.AddCollectible();
        gameObject.SetActive(false);
    }
}
