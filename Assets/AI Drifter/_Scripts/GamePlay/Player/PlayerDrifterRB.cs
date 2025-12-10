using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrifterRB : MonoBehaviour
{
    public float MoveSpeed = 50f;     // forward force
    public float MaxSpeed = 15f;      // maximum velocity
    public float Drag = 0.98f;        // how quickly force reduces
    public float SteerAngle = 20f;    // rotation based on velocity
    public float Traction = 1f;       // how much the car aligns forward

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);  // helps drifting stability
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
        ApplyDrag();
        ApplyTraction();
    }

    private void HandleMovement()
    {
        // Forward propulsion
        rb.AddForce(transform.forward * MoveSpeed, ForceMode.Acceleration);

        // Clamp max speed
        if (rb.velocity.magnitude > MaxSpeed)
            rb.velocity = rb.velocity.normalized * MaxSpeed;
    }

    private void HandleSteering()
    {
        float steerInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(steerInput) > 0.01f)
        {
            float turn = steerInput * SteerAngle * rb.velocity.magnitude * Time.fixedDeltaTime;

            // Manual rotation
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));
        }
    }

    private void ApplyDrag()
    {
        rb.velocity *= Drag;
    }

    private void ApplyTraction()
    {
        // Drift correction force
        Vector3 alignedVelocity = Vector3.Lerp(
            rb.velocity.normalized,
            transform.forward,
            Traction * Time.fixedDeltaTime
        );

        rb.velocity = alignedVelocity * rb.velocity.magnitude;
    }
}
