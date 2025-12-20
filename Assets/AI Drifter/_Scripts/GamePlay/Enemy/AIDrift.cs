using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Mad,
    Smart,
    Aggressive
}

public class AIDrift : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] private EnemyType enemyType;
    public Transform target;
    [SerializeField] private float repathInterval = 0.2f;
    [SerializeField] private GameObject gunTank;
    [SerializeField] private Transform shootingPoint;

    [SerializeField] private int shootInterval;
    [SerializeField] private GameObject gola;
    [SerializeField] private float shootingDistance = 40;
    private NavMeshAgent agent;
    private float timer;

    // Internal tuned values (NO inspector control)
    private float turnSpeed;
    private float maxAngle;
    private float oversteerMultiplier;
    private float chaosFreq;
    private float chaosStrength;
    private bool isShootEnabled = false;
    private float previousShoot = 0;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        ApplyEnemyPreset();
    }

    void Start()
    {
        if (target)
            agent.SetDestination(target.position);
    }

    void ApplyEnemyPreset()
    {
        switch (enemyType)
        {
            // =========================
            // 😡 MAD AI — INSANE DRIFT
            // =========================
            case EnemyType.Mad:
                turnSpeed = 15f;
                maxAngle = 90f;
                oversteerMultiplier = 1.8f;
                // chaosFreq = 10f;
                // chaosStrength = 14f;
                chaosFreq = 10f;
                chaosStrength = 3f;

                agent.speed = 40f;
                agent.acceleration = 50f;
                agent.angularSpeed = 250f;
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
                break;

            // =========================
            // 🧠 SMART AI — NO DRIFT
            // =========================
            case EnemyType.Smart:
                turnSpeed = 6f;
                maxAngle = 40f; // IMPORTANT → disables drift
                oversteerMultiplier = 1f;
                chaosFreq = 0f;
                chaosStrength = 0f;

                agent.speed = 35f;
                agent.acceleration = 25f;
                // agent.speed = 30f;
                // agent.acceleration = 20f;
                agent.angularSpeed = 150f;
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
                break;
            

            case EnemyType.Aggressive:
                turnSpeed = 6f;
                maxAngle = 40f; // IMPORTANT → disables drift
                oversteerMultiplier = 1f;
                chaosFreq = 0f;
                chaosStrength = 0f;

                agent.speed = 30f;
                agent.acceleration = 30f;
                // agent.speed = 30f;
                // agent.acceleration = 20f;
                agent.angularSpeed = 150f;
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
                isShootEnabled = true;
                break;
        }
    }

    void Update()
    {
        if (!target) return;

        timer += Time.deltaTime;
        if (timer >= repathInterval)
        {
            timer = 0f;
            agent.SetDestination(target.position);
        }

        if (agent.desiredVelocity.sqrMagnitude < 0.01f)
            return;

        Vector3 desiredDir = agent.desiredVelocity.normalized;

        // =========================
        // SMART AI → DIRECT ROTATION
        // =========================
        if (enemyType == EnemyType.Smart)
        {
            Quaternion targetRot = Quaternion.LookRotation(desiredDir, Vector3.up);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                turnSpeed * Time.deltaTime
            );
            return;
        }

        // =========================
        // MAD AI → OVERDRIFT
        // =========================
        float angle = Vector3.SignedAngle(
            transform.forward,
            desiredDir,
            Vector3.up
        );

        angle *= oversteerMultiplier;

        float chaos = Mathf.Sin(Time.time * chaosFreq) * chaosStrength;
        angle += chaos;

        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);

        Quaternion driftRot = Quaternion.Euler(
            0f,
            transform.eulerAngles.y + angle,
            0f
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            driftRot,
            turnSpeed * Time.deltaTime
        );

        if(isShootEnabled)
        {
            ShootPlayer();
        }
    }   

    void ShootPlayer()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z);
        Vector3 targetDirection = (targetPosition - transform.position).normalized;
        
        gunTank.transform.forward = Vector3.Lerp(gunTank.transform.forward,targetDirection,Time.deltaTime * 50f);

        previousShoot += Time.deltaTime;
        Vector3 distance = transform.position - target.transform.position;
        float distanceMagnitude = distance.magnitude;
        // Debug.Log("Distance between target and enemy is : "+distanceMagnitude);
        if(previousShoot > shootInterval && distanceMagnitude <= shootingDistance)  
        {
            previousShoot = 0;
            // Shoot bullet here
            GameObject bomb = Instantiate(gola,shootingPoint.position,quaternion.identity);
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.AddForce(shootingPoint.forward * 50f, ForceMode.Impulse);
        }
    }

#if UNITY_EDITOR
    // Auto-update when changing enum in Inspector
    void OnValidate()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (agent) ApplyEnemyPreset();
    }
#endif
}
