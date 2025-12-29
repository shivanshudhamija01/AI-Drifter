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

    [Header("Audio ")]
    [SerializeField] private AudioSource audioSource;

    private Vector3 MoveForce;
    private float steerInput;

    // independent roll angles (prevents flipping)
    private float flRoll, frRoll, rlRoll, rrRoll;
    private float previousInput = 0;

    void Update()
    {
        steerInput = Input.GetAxis("Horizontal");
        if (steerInput != previousInput)
        {
            if (steerInput == 0)
            {
                audioSource.Stop();
            }
            else
            {
                previousInput = steerInput;
                audioSource.Play();
            }
        }
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
