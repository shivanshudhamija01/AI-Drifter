using UnityEngine;
using UnityEngine.AI;

public class AIDrift : MonoBehaviour
{
    [SerializeField] private float driftTurnSpeed = 3f;   // how slowly/loosely the AI turns (drift feel)
    [SerializeField] private float maxRotationAngle = 45f; // how much the AI can lean into the turn
    [SerializeField] private Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;   // IMPORTANT — we rotate manually
    }

    void Update()
    {
        agent.SetDestination(target.transform.position);
        // If AI is not moving, no rotation needed
        if (agent.desiredVelocity.sqrMagnitude < 0.001f)
            return;

        // Direction NavMesh wants to go
        Vector3 desiredDir = agent.desiredVelocity.normalized;

        // Find the angle difference between current forward and desired direction
        float angle = Vector3.SignedAngle(transform.forward, desiredDir, Vector3.up);

        // Clamp the rotation angle for a drifting feel (car leans but does not snap)
        angle = Mathf.Clamp(angle, -maxRotationAngle, maxRotationAngle);

        // Smooth drifting-style rotation
        Quaternion targetRot = Quaternion.Euler(0, transform.eulerAngles.y + angle, 0);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            driftTurnSpeed * Time.deltaTime
        );
    }
}
