// using UnityEngine;

// public class PlayerDrifter : MonoBehaviour
// {
//     [Header("Movement")]
//     public float MoveSpeed = 50f;
//     public float MaxSpeed = 15f;
//     public float Drag = 0.98f;
//     public float SteerAngle = 20f;
//     public float Traction = 1f;

//     [Header("Wheel Transforms")]
//     [SerializeField] private Transform frontLeftWheel;
//     [SerializeField] private Transform frontRightWheel;
//     [SerializeField] private Transform rearLeftWheel;
//     [SerializeField] private Transform rearRightWheel;

//     [Header("Wheel Visual Settings")]
//     [SerializeField] private float wheelRadius = 0.35f;
//     [SerializeField] private float maxSteerAngle = 30f;


//     private Vector3 MoveForce;
//     private float steerInput;
//     private float wheelRollAngle = 0f;

//     void Update()
//     {
//         // =========================
//         // INPUT
//         // =========================
//         steerInput = Input.GetAxis("Horizontal");

//         // =========================
//         // MOVEMENT
//         // =========================
//         MoveForce += transform.forward * MoveSpeed * Time.deltaTime;
//         transform.position += MoveForce * Time.deltaTime;

//         // =========================
//         // STEERING
//         // =========================
//         transform.Rotate(
//             Vector3.up * steerInput * MoveForce.magnitude * SteerAngle * Time.deltaTime
//         );

//         // =========================
//         // DRAG & SPEED LIMIT
//         // =========================
//         MoveForce *= Drag;
//         MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

//         // =========================
//         // TRACTION (DRIFT CONTROL)
//         // =========================
//         MoveForce = Vector3.Lerp(
//             MoveForce.normalized,
//             transform.forward,
//             Traction * Time.deltaTime
//         ) * MoveForce.magnitude;

//         // =========================
//         // WHEEL VISUALS
//         // =========================
//         WheelRotation();
//     }

//     void WheelRotation()
//     {
//         float speed = MoveForce.magnitude;

//         // =========================
//         // WHEEL ROLLING
//         // =========================
//         float rotationAngle = (speed / wheelRadius) * Mathf.Rad2Deg * Time.deltaTime;

//         RotateWheel(frontLeftWheel, rotationAngle);
//         RotateWheel(frontRightWheel, rotationAngle);
//         RotateWheel(rearLeftWheel, rotationAngle);
//         RotateWheel(rearRightWheel, rotationAngle);

//         // =========================
//         // FRONT WHEEL STEERING
//         // =========================
//         float steerAngle = steerInput * maxSteerAngle;

//         SetSteer(frontLeftWheel, steerAngle);
//         SetSteer(frontRightWheel, steerAngle);
//     }

//     // void RotateWheel(Transform wheel, float angle)
//     // {
//     //     if (!wheel) return;
//     //     wheel.Rotate(Vector3.right, angle, Space.Self);
//     // }

//     // void SetSteer(Transform wheel, float steerAngle)
//     // {
//     //     if (!wheel) return;

//     //     Vector3 euler = wheel.localEulerAngles;
//     //     euler.y = steerAngle;
//     //     wheel.localEulerAngles = euler;
//     // }
//     // void RotateWheel(Transform wheel, float rollAngle)
//     // {
//     //     if (!wheel) return;

//     //     // Keep existing Y steering — only roll on X, force Z = 0
//     //     Quaternion current = wheel.localRotation;

//     //     float steerY = current.eulerAngles.y;  // keep steering
//     //     float rollX = current.eulerAngles.x + rollAngle;

//     //     wheel.localRotation = Quaternion.Euler(rollX, steerY, 0f);
//     // }

//     // void SetSteer(Transform wheel, float steerAngle)
//     // {
//     //     if (!wheel) return;

//     //     // Only apply steering to Y axis — keep roll — force Z = 0
//     //     Quaternion current = wheel.localRotation;

//     //     float rollX = current.eulerAngles.x;

//     //     wheel.localRotation = Quaternion.Euler(rollX, steerAngle, 0f);
//     // }
//     void RotateWheel(Transform wheel, float rollDelta)
//     {
//         if (!wheel) return;

//         // accumulate roll (never read from Unity's Euler)
//         wheelRollAngle += rollDelta;

//         // build rotation: X = roll, Y = steering, Z always 0
//         wheel.localRotation = Quaternion.Euler(wheelRollAngle, steerInput * maxSteerAngle, 0f);
//     }

//     void SetSteer(Transform wheel, float steerAngle)
//     {
//         if (!wheel) return;

//         // keep current roll, only change steering
//         Vector3 e = wheel.localEulerAngles;
//         wheel.localRotation = Quaternion.Euler(wheelRollAngle, steerAngle, 0f);
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

    private Vector3 MoveForce;
    private float steerInput;

    // independent roll angles (prevents flipping)
    private float flRoll, frRoll, rlRoll, rrRoll;

    void Update()
    {
        steerInput = Input.GetAxis("Horizontal");

        // ---- MOVEMENT ----
        MoveForce += transform.forward * MoveSpeed * Time.deltaTime;
        transform.position += MoveForce * Time.deltaTime;

        // ---- STEERING ----
        transform.Rotate(
            Vector3.up * steerInput * MoveForce.magnitude * SteerAngle * Time.deltaTime
        );

        // ---- DRAG / LIMIT ----
        MoveForce *= Drag;
        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

        // ---- TRACTION ----
        MoveForce = Vector3.Lerp(
            MoveForce.normalized,
            transform.forward,
            Traction * Time.deltaTime
        ) * MoveForce.magnitude;

        // ---- WHEELS ----
        WheelRotation();
    }

    void WheelRotation()
    {
        float speed = MoveForce.magnitude;
        float rollDelta = (speed / wheelRadius) * Mathf.Rad2Deg * Time.deltaTime;

        // accumulate roll separately for each wheel
        flRoll += rollDelta;
        frRoll += rollDelta;
        rlRoll += rollDelta;
        rrRoll += rollDelta;

        float steerAngle = steerInput * maxSteerAngle;

        // ----- FRONT (roll + steer) -----
        SetWheel(frontLeftWheel, flRoll, steerAngle);
        SetWheel(frontRightWheel, frRoll, steerAngle);

        // ----- REAR (roll only, NO steer) -----
        SetWheel(rearLeftWheel, rlRoll, 0f);
        SetWheel(rearRightWheel, rrRoll, 0f);
    }

    void SetWheel(Transform wheel, float rollX, float steerY)
    {
        if (!wheel) return;

        // X = roll, Y = steering, Z locked to 0
        wheel.localRotation = Quaternion.Euler(rollX, steerY, 0f);
    }
}
