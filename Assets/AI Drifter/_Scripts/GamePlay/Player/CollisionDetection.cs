using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
  [Header("Capsule Settings")]
    [SerializeField] private float capsuleHeight = 1.8f;
    [SerializeField] private float capsuleRadius = 0.4f;
    [SerializeField] private float detectionDistance = 3f;

    [Header("Detection")]
    [SerializeField] private LayerMask obstacleLayers;

    [Header("Debug")]
    [SerializeField] private bool drawDebug = true;

    [Header("Re-Hit Delay")]
    [SerializeField] private float sameObjectCooldown = 1.5f;

    public bool ObstacleDetected { get; private set; }
    public RaycastHit HitInfo { get; private set; }
    private Dictionary<Collider, float> hitCooldowns = new Dictionary<Collider, float>();
    private PlayerHealth playerHealth;


    void Start()
    {
        playerHealth = this.gameObject.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        DetectObstacle();
    }

    void DetectObstacle()
    {
        Vector3 center = transform.position + Vector3.up * capsuleHeight * 0.5f;

        Vector3 point1 = center + transform.forward * (capsuleHeight * 0.2f - capsuleRadius);
        Vector3 point2 = center - transform.forward * (capsuleHeight * 0.2f - capsuleRadius);

        ObstacleDetected = Physics.CapsuleCast(
            point1,
            point2,
            capsuleRadius,
            transform.forward,
            out RaycastHit hit,
            detectionDistance,
            obstacleLayers,
            QueryTriggerInteraction.Ignore
        );

        if (!ObstacleDetected)
            return;

        Collider col = hit.collider;
        if (col == null)
        {
            Debug.Log("col is null ");
            return ;
        }
        
        if (hitCooldowns.TryGetValue(col, out float lastHitTime))
        {
            if (Time.time - lastHitTime < sameObjectCooldown)
                return; 
        }
        hitCooldowns[col] = Time.time;
        OnCapsuleCollisionEnter(hit);
        
        // Runtime ray (Game view)
        if (drawDebug)
        {
            Color rayColor = ObstacleDetected ? Color.red : Color.green;
            Debug.DrawRay(center, transform.forward * detectionDistance, rayColor);
        }
    }
    void OnCapsuleCollisionEnter(RaycastHit hit)
    {
        // Debug.Log($"Capsule ENTER: {hit.collider.name}");
        // Here check for the different different types of tag used in the game scene
        if(hit.collider.gameObject.CompareTag(Tag.COLLECTIBLE_HEALTH_TAG))
        {
            playerHealth.UpdateHealth(100);
        }
        else if(hit.collider.gameObject.CompareTag(Tag.ENEMY_TAG))
        {
            playerHealth.UpdateHealth(Constants.ENEMY_CAR_HIT_IMPACT);
        } 
        else if(hit.collider.gameObject.CompareTag(Tag.GOLA_TAG))
        {
            playerHealth.UpdateHealth(Constants.BLAST_IMPACT);
        }
        else
        {
            playerHealth.UpdateHealth(-100);
        }
    }

    // =========================
    // SCENE VIEW VISUALIZATION
    // =========================
    void OnDrawGizmos()
    {
        if (!drawDebug) return;

        Gizmos.color = ObstacleDetected ? Color.red : Color.green;

        Vector3 center = transform.position + Vector3.up * capsuleHeight * 0.5f;

        Vector3 point1 = center + transform.forward * (capsuleHeight * 0.2f - capsuleRadius);
        Vector3 point2 = center - transform.forward * (capsuleHeight * 0.2f - capsuleRadius);

        // Draw capsule start
        DrawCapsule(point1, point2, capsuleRadius);

        // Draw direction
        Gizmos.DrawLine(center, center + transform.forward * detectionDistance);

        // Draw hit point
        if (ObstacleDetected)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(HitInfo.point, 0.1f);
        }
    }

    // =========================
    // CAPSULE DRAWING
    // =========================
    void DrawCapsule(Vector3 point1, Vector3 point2, float radius)
    {
        Gizmos.DrawWireSphere(point1, radius);
        Gizmos.DrawWireSphere(point2, radius);
        Gizmos.DrawLine(point1 + Vector3.forward * radius, point2 + Vector3.forward * radius);
        Gizmos.DrawLine(point1 - Vector3.forward * radius, point2 - Vector3.forward * radius);
        Gizmos.DrawLine(point1 + Vector3.right * radius, point2 + Vector3.right * radius);
        Gizmos.DrawLine(point1 - Vector3.right * radius, point2 - Vector3.right * radius);
    }
}
