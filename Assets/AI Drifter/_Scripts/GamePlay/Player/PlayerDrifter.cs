// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerDrifter : MonoBehaviour
// {
//     public float MoveSpeed = 50;
//     public float MaxSpeed = 15;
//     public float Drag = 0.98f;
//     public float SteerAngle = 20;
//     public float Traction = 1;
    
//     [SerializeField] private Transform frontLeftWheel;
//     [SerializeField] private Transform frontRightWheel;
//     [SerializeField] private Transform rearLeftWheel;
//     [SerializeField] private Transform rearRightWheel;
//     // Variables
//     [Header("Visual Settings")]
//     [SerializeField] private float wheelRadius = 0.35f;
//     [SerializeField] private float maxSteerAngle = 30f;

//     private Vector3 MoveForce;

//     // Update is called once per frame
//     void Update() {

//         // Moving
//         MoveForce += transform.forward * MoveSpeed *  Time.deltaTime;
//         transform.position += MoveForce * Time.deltaTime;

//         // Steering
//         float steerInput = Input.GetAxis("Horizontal");
//         transform.Rotate(Vector3.up * steerInput * MoveForce.magnitude * SteerAngle * Time.deltaTime);

//         // Drag and max speed limit
//         MoveForce *= Drag;
//         MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

//         // Traction
//         Debug.DrawRay(transform.position, MoveForce.normalized * 3);
//         Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
//         MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;

//         WheelRotation();
//     }
//     void WheelRotation()
//     {
//          float speed = MoveSpeed;

//         // =========================
//         // ROLLING ROTATION
//         // =========================
//         float rotationAngle = (speed / wheelRadius) * Mathf.Rad2Deg * Time.deltaTime;

//         RotateWheel(frontLeftWheel, rotationAngle);
//         RotateWheel(frontRightWheel, rotationAngle);
//         RotateWheel(rearLeftWheel, rotationAngle);
//         RotateWheel(rearRightWheel, rotationAngle);

//         // =========================
//         // STEERING (Front Wheels)
//         // =========================
//         if (speed > 0.1f)
//         {
//             Vector3 localVel = transform.InverseTransformDirection(agent.velocity);
//             float steer = Mathf.Clamp(localVel.x * maxSteerAngle, -maxSteerAngle, maxSteerAngle);

//             SetSteer(frontLeftWheel, steer);
//             SetSteer(frontRightWheel, steer);
//         }
//     }

//     void RotateWheel(Transform wheel, float angle)
//     {
//         if (!wheel) return;
//         wheel.Rotate(Vector3.right, angle, Space.Self);
//     }

//     void SetSteer(Transform wheel, float steerAngle)
//     {
//         if (!wheel) return;

//         Vector3 euler = wheel.localEulerAngles;
//         euler.y = steerAngle;
//         wheel.localEulerAngles = euler;
//     }
    
// }
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
    [SerializeField] private int shootingDistance = 40;

    private Vector3 MoveForce;
    private float steerInput;

    void Update()
    {
        // =========================
        // INPUT
        // =========================
        steerInput = Input.GetAxis("Horizontal");

        // =========================
        // MOVEMENT
        // =========================
        MoveForce += transform.forward * MoveSpeed * Time.deltaTime;
        transform.position += MoveForce * Time.deltaTime;

        // =========================
        // STEERING
        // =========================
        transform.Rotate(
            Vector3.up * steerInput * MoveForce.magnitude * SteerAngle * Time.deltaTime
        );

        // =========================
        // DRAG & SPEED LIMIT
        // =========================
        MoveForce *= Drag;
        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

        // =========================
        // TRACTION (DRIFT CONTROL)
        // =========================
        MoveForce = Vector3.Lerp(
            MoveForce.normalized,
            transform.forward,
            Traction * Time.deltaTime
        ) * MoveForce.magnitude;

        // =========================
        // WHEEL VISUALS
        // =========================
        WheelRotation();
    }

    void WheelRotation()
    {
        float speed = MoveForce.magnitude;

        // =========================
        // WHEEL ROLLING
        // =========================
        float rotationAngle = (speed / wheelRadius) * Mathf.Rad2Deg * Time.deltaTime;

        RotateWheel(frontLeftWheel, rotationAngle);
        RotateWheel(frontRightWheel, rotationAngle);
        RotateWheel(rearLeftWheel, rotationAngle);
        RotateWheel(rearRightWheel, rotationAngle);

        // =========================
        // FRONT WHEEL STEERING
        // =========================
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
