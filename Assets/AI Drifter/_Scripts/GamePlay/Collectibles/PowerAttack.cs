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
        PlayerServices.Instance.OnCollectiblePicked.Invoke(Enums.Collectibles.powerAttack);
        PlayerServices.Instance.OnPowerAttack.Invoke();
        AudioServices.Instance.PlayAudio.Invoke(Enums.Audios.powerAttack);
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
