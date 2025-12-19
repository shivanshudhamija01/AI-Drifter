using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, ICollectible
{
    [SerializeField] private float duration;
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.shield;
    }

    public void OnCollected(GameObject collector)
    {
        if(collector.TryGetComponent<CollisionDetection>(out CollisionDetection component))
        {
            component.StartCoroutine(DisableCollisionDetection(component));
        }
        Destroy(gameObject);
    }
    IEnumerator DisableCollisionDetection(CollisionDetection component)
    {
        component.enabled = false;
        yield return new WaitForSeconds(duration);
        component.enabled = true;
    }
}
