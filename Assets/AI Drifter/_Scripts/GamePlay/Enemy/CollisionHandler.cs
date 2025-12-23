using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem blastEffect;
    private AIDrift aIDrift;
    private CollisionHandler collisionHandler;
    void Start()
    {
        if (TryGetComponent<AIDrift>(out AIDrift component))
        {
            aIDrift = component;
        }
        collisionHandler = GetComponent<CollisionHandler>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (aIDrift.target != null)
        {
            float distance = Vector3.Distance(transform.position, aIDrift.target.position);
            if (distance < 30f && !other.collider.gameObject.CompareTag("Enemy"))
            {
                blastEffect.Play();
                EnemyServices.Instance.OnObstacleHit.Invoke(this.gameObject);
                StartCoroutine(DeactivateEnemy());
            }
        }
    }
    IEnumerator DeactivateEnemy()
    {
        if (aIDrift != null)
        {
            aIDrift.enabled = false;
            collisionHandler.enabled = false;
            Debug.Log("Enemy Disabled");
            yield return new WaitForSeconds(6f);
            Debug.Log("Enemy Enabled");
            aIDrift.enabled = true;
            collisionHandler.enabled = true;
        }
        yield return null;
    }
}
