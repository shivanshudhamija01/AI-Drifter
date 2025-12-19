using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemical : MonoBehaviour, ICollectible
{
    [SerializeField] private float impactTime = 1;
    [SerializeField] private float healthToBeReducedBy = 1;
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.chemical;
    }

    public void OnCollected(GameObject collector)
    {
        PlayerServices.Instance.OnCollectiblePicked.Invoke(Enums.Collectibles.chemical);   
        if(collector.TryGetComponent<PlayerHealth>(out PlayerHealth component))
        {
            component.StartCoroutine(ChemicalEffect(component));
        }

        Destroy(gameObject);
    }
    IEnumerator ChemicalEffect(PlayerHealth component)
    {
        float healthReducePerSecondIs = healthToBeReducedBy / impactTime;
        float elapsedTime = 0f;

        while(elapsedTime < impactTime)
        {
            float deltaHealth = -1 * healthReducePerSecondIs * Time.deltaTime;
            component.UpdateHealth(deltaHealth);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
