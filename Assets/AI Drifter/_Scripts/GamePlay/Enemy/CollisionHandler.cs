using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem blastEffect;
    [SerializeField] private GameObject visuals;
    [SerializeField] private Collider colliderField;
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
    void OnEnable()
    {
        PlayerServices.Instance.OnPowerAttack.AddListener(DeactivateEnemyForWhile);
        PlayerServices.Instance.OnPlayerDead.AddListener(PlayerDead);
    }
    void OnDisable()
    {
        PlayerServices.Instance.OnPowerAttack.RemoveListener(DeactivateEnemyForWhile);
        PlayerServices.Instance.OnPlayerDead.RemoveListener(PlayerDead);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (aIDrift.target != null)
        {
            float distance = Vector3.Distance(transform.position, aIDrift.target.position);
            if (distance < 30f && !other.collider.gameObject.CompareTag("Enemy") && !other.collider.gameObject.CompareTag("Collectible"))
            {
                blastEffect.Play();
                AudioServices.Instance.PlayAudio.Invoke(Enums.Audios.blast);
                EnemyServices.Instance.OnObstacleHit.Invoke(this.gameObject);
                StartCoroutine(DeactivateEnemy());
            }
        }
    }

    private void DeactivateEnemyForWhile()
    {
        StartCoroutine(DeactivateEnemy());
    }
    IEnumerator DeactivateEnemy()
    {
        if (aIDrift == null) yield break;

        aIDrift.enabled = false;
        collisionHandler.enabled = false;
        colliderField.enabled = false;
        visuals.SetActive(false);
        Debug.Log("Enemy Disabled");
        yield return new WaitForSeconds(6f);
        Debug.Log("Enemy Enabled");
        aIDrift.enabled = true;
        collisionHandler.enabled = true;
        visuals.SetActive(true);
        colliderField.enabled = true;
    }

    void PlayerDead()
    {
        StopAllCoroutines();

        if (aIDrift) aIDrift.enabled = false;
        if (colliderField) colliderField.enabled = false;
        if (collisionHandler) collisionHandler.enabled = false;

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Debug.Log("Enemy stopped due to player death");
    }
}
