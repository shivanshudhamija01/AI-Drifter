using UnityEngine;
public interface ICollectible
{
    Enums.Collectibles GetCollectibleType();
    void OnCollected(GameObject collector);
}
