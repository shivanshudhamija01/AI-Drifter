
using UnityEngine;

public class PlayerDrifter : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 50f;
    public float MaxSpeed = 15f;
    public float Drag = 0.98f;
    public float SteerAngle = 20f;
    public float Traction = 1f;

    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform rearLeftWheel;
    [SerializeField] private Transform rearRightWheel;

    [Header("Wheel Visual Settings")]
    [SerializeField] private float wheelRadius = 0.35f;
    [SerializeField] private float maxSteerAngle = 30f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Drift Settings")]
    [SerializeField] private float driftLeanAngle = 15f;
    [SerializeField] private float leanSpeed = 5f;
    [SerializeField] private Transform carBodyTransform;

    private Vector3 MoveForce;
    private float steerInput;
    private float flRoll, frRoll, rlRoll, rrRoll;
    private float previousInput = 0;
    private float currentLeanAngle = 0f;
    private Vector3 lastPosition;
    private Vector3 lastForward;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.useGravity = false;

        lastPosition = transform.position;
        lastForward = transform.forward;

        // If no car body transform assigned, use the main transform
        if (carBodyTransform == null)
        {
            carBodyTransform = transform;
        }
    }

    void Update()
    {
        steerInput = GetSteerInput();

        // Audio handling
        if (steerInput != previousInput)
        {
            if (steerInput == 0)
            {
                audioSource.Stop();
            }
            else
            {
                if (previousInput == 0)
                {
                    audioSource.Play();
                }
            }
            previousInput = steerInput;
        }

        // Apply drift lean visual
        ApplyDriftLean();
    }

    void FixedUpdate()
    {
        // Store previous state for drift calculation
        lastPosition = transform.position;
        lastForward = transform.forward;

        // ---- MOVEMENT ----
        MoveForce += transform.forward * MoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + MoveForce * Time.fixedDeltaTime);

        // ---- STEERING ----
        float rotationAmount = steerInput * MoveForce.magnitude * SteerAngle * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0f, rotationAmount, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);

        // ---- DRAG / LIMIT ----
        MoveForce *= Drag;
        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

        // ---- DRIFT PHYSICS ----
        // Reduce traction when steering to allow sideways sliding
        float driftFactor = Mathf.Abs(steerInput) * 0.5f;
        float adjustedTraction = Traction * (1f - driftFactor);

        MoveForce = Vector3.Lerp(
            MoveForce.normalized,
            transform.forward,
            adjustedTraction * Time.fixedDeltaTime
        ) * MoveForce.magnitude;

        // --- WHEELS ----
        WheelRotation();
    }

    void ApplyDriftLean()
    {
        // Calculate drift angle (difference between movement direction and facing direction)
        Vector3 velocityDirection = MoveForce.normalized;
        float driftAngle = Vector3.SignedAngle(transform.forward, velocityDirection, Vector3.up);

        // Target lean based on steering input and speed
        float speedFactor = Mathf.Clamp01(MoveForce.magnitude / MaxSpeed);
        float targetLean = -steerInput * driftLeanAngle * speedFactor;

        // Add extra lean based on actual drift angle
        targetLean += driftAngle * 0.3f;

        // Smoothly interpolate to target lean
        currentLeanAngle = Mathf.Lerp(currentLeanAngle, targetLean, Time.deltaTime * leanSpeed);

        // Apply lean rotation to car body
        if (carBodyTransform != null && carBodyTransform != transform)
        {
            carBodyTransform.localRotation = Quaternion.Euler(0f, 0f, currentLeanAngle);
        }
    }

    float GetSteerInput()
    {
        float input = Input.GetAxis("Horizontal");

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float halfScreenWidth = Screen.width / 2f;

            if (touch.position.x < halfScreenWidth)
            {
                input = -1f;
            }
            else
            {
                input = 1f;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            float halfScreenWidth = Screen.width / 2f;
            if (Input.mousePosition.x < halfScreenWidth)
            {
                input = -1f;
            }
            else
            {
                input = 1f;
            }
        }
        return input;
    }

    void WheelRotation()
    {
        float speed = MoveForce.magnitude;
        float rollDelta = (speed / wheelRadius) * Mathf.Rad2Deg * Time.fixedDeltaTime;

        flRoll += rollDelta;
        frRoll += rollDelta;
        rlRoll += rollDelta;
        rrRoll += rollDelta;

        float steerAngle = steerInput * maxSteerAngle;

        // Front wheels with steering
        SetWheel(frontLeftWheel, flRoll, steerAngle);
        SetWheel(frontRightWheel, frRoll, steerAngle);

        // Rear wheels without steering
        SetWheel(rearLeftWheel, rlRoll, 0f);
        SetWheel(rearRightWheel, rrRoll, 0f);
    }

    void SetWheel(Transform wheel, float rollX, float steerY)
    {
        if (!wheel) return;
        wheel.localRotation = Quaternion.Euler(rollX, steerY, 0f);
    }
}