using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrifterRB : MonoBehaviour
{
    // public float MoveSpeed = 50f;     // forward force
    // public float MaxSpeed = 15f;      // maximum velocity
    // public float Drag = 0.98f;        // how quickly force reduces
    // public float SteerAngle = 20f;    // rotation based on velocity
    // public float Traction = 1f;       // how much the car aligns forward

    // private Rigidbody rb;

    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    //     rb.centerOfMass = new Vector3(0, -0.5f, 0);  // helps drifting stability
    // }

    // void FixedUpdate()
    // {
    //     HandleMovement();
    //     HandleSteering();
    //     ApplyDrag();
    //     ApplyTraction();
    // }

    // private void HandleMovement()
    // {
    //     // Forward propulsion
    //     rb.AddForce(transform.forward * MoveSpeed, ForceMode.Acceleration);

    //     // Clamp max speed
    //     if (rb.velocity.magnitude > MaxSpeed)
    //         rb.velocity = rb.velocity.normalized * MaxSpeed;
    // }

    // private void HandleSteering()
    // {
    //     float steerInput = Input.GetAxis("Horizontal");

    //     if (Mathf.Abs(steerInput) > 0.01f)
    //     {
    //         float turn = steerInput * SteerAngle * rb.velocity.magnitude * Time.fixedDeltaTime;

    //         // Manual rotation
    //         rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));
    //     }
    // }

    // private void ApplyDrag()
    // {
    //     rb.velocity *= Drag;
    // }

    // private void ApplyTraction()
    // {
    //     // Drift correction force
    //     Vector3 alignedVelocity = Vector3.Lerp(
    //         rb.velocity.normalized,
    //         transform.forward,
    //         Traction * Time.fixedDeltaTime
    //     );

    //     rb.velocity = alignedVelocity * rb.velocity.magnitude;
    // }
    [Header("Movement")]
    public float motorForce = 1500f;
    public float maxSpeed = 15f;
    public float steerStrength = 3.5f;
    public float traction = 0.8f;
    public float sidewaysFriction = 6f;

    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform rearLeftWheel;
    [SerializeField] private Transform rearRightWheel;

    [Header("Wheel Visuals")]
    [SerializeField] private float wheelRadius = 0.35f;
    [SerializeField] private float maxSteerAngle = 30f;

    private Rigidbody rb;
    private float steerInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // improves stability
    }

    void FixedUpdate()
    {
        steerInput = Input.GetAxis("Horizontal");

        HandleMovement();
        HandleSteering();
        ApplyDrift();
        LimitSpeed();
    }

    void Update()
    {
        WheelVisuals();
    }

    // =========================
    // MOVEMENT (Physics)
    // =========================
    void HandleMovement()
    {
        rb.AddForce(transform.forward * motorForce * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    // =========================
    // STEERING (Physics)
    // =========================
    void HandleSteering()
    {
        if (rb.velocity.magnitude < 0.5f) return;

        float steerAmount = steerInput * steerStrength * rb.velocity.magnitude;
        Quaternion turn = Quaternion.Euler(0f, steerAmount, 0f);
        rb.MoveRotation(rb.rotation * turn);
    }

    // =========================
    // DRIFT / TRACTION
    // =========================
    void ApplyDrift()
    {
        Vector3 forwardVel = transform.forward * Vector3.Dot(rb.velocity, transform.forward);
        Vector3 sideVel = transform.right * Vector3.Dot(rb.velocity, transform.right);

        rb.velocity = Vector3.Lerp(
            sideVel,
            Vector3.zero,
            sidewaysFriction * Time.fixedDeltaTime
        ) + forwardVel * traction;
    }

    // =========================
    // SPEED LIMIT
    // =========================
    void LimitSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    // =========================
    // WHEEL VISUALS
    // =========================
    void WheelVisuals()
    {
        float speed = rb.velocity.magnitude;
        float rotationAngle = (speed / wheelRadius) * Mathf.Rad2Deg * Time.deltaTime;

        RotateWheel(frontLeftWheel, rotationAngle);
        RotateWheel(frontRightWheel, rotationAngle);
        RotateWheel(rearLeftWheel, rotationAngle);
        RotateWheel(rearRightWheel, rotationAngle);

        float steerAngle = steerInput * maxSteerAngle;
        SetSteer(frontLeftWheel, steerAngle);
        SetSteer(frontRightWheel, steerAngle);
    }

    void RotateWheel(Transform wheel, float angle)
    {
        if (!wheel) return;
        wheel.Rotate(Vector3.right, angle, Space.Self);
    }

    void SetSteer(Transform wheel, float steerAngle)
    {
        if (!wheel) return;
        Vector3 euler = wheel.localEulerAngles;
        euler.y = steerAngle;
        wheel.localEulerAngles = euler;
    }

}
