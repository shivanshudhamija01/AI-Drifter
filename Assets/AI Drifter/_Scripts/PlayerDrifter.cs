using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrifter : MonoBehaviour
{

    [SerializeField] private float driftSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deacceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxRotation;
    float speed = 0;
    float rotation = 0;
    private float x = 0;
    private float y = 0;
    void Start()
    {
        
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        ForwardMovement(y);
        Drift(x);
    }

    private void ForwardMovement(float y)
    {
        if(y!=0)
        {
            speed += acceleration * y * Time.deltaTime;
            speed = Mathf.Clamp(speed, -1*maxSpeed, maxSpeed);
        }
        else
        {
            if(speed<0)
            {
                speed += deacceleration * Time.deltaTime;
                speed = Mathf.Min(0,speed);
            }
            else
            {
                speed -= deacceleration * Time.deltaTime;
                speed = Mathf.Max(0,speed);
            }
            
        }
        Vector3 speedVector = transform.forward * speed;
        Debug.Log("Value of speed : "+ speed);
        transform.position += speedVector;
    }
    private void Drift(float x)
    {
        if(x!=0)
        {
            rotation += driftSpeed * x * Time.deltaTime;
        }
        
        Quaternion rotationVector = Quaternion.Euler(0,rotation,0);

        transform.rotation = Quaternion.Slerp(transform.rotation,rotationVector,driftSpeed*Time.deltaTime);
    }
}
