using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour, ICollectible
{
   [SerializeField] private float boostAmount = 5f;
    [SerializeField] private float duration = 3f;

    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.speedUp;
    }

    public void OnCollected(GameObject collector)
    {
        PlayerDrifter drifter = collector.GetComponent<PlayerDrifter>();
        if (drifter != null)
        {
             PlayerServices.Instance.OnCollectiblePicked.Invoke(Enums.Collectibles.speedUp);
            drifter.StartCoroutine(SpeedBoost(drifter));
        }

        Destroy(gameObject);
    }

    IEnumerator SpeedBoost(PlayerDrifter drifter)
    {
        drifter.MoveSpeed += boostAmount;
        drifter.MaxSpeed += boostAmount;
        yield return new WaitForSeconds(duration);
        drifter.MoveSpeed -= boostAmount;
        drifter.MaxSpeed -= boostAmount;
    }
}
