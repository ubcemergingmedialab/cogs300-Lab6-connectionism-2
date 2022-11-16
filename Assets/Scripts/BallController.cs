using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallController : MonoBehaviour
{

    public float speed = 0.5f;


    public float maxSpeed;

    public float velocity;

    public float acceleration = 10f;

    public float maxMotorTorque;

    protected Rigidbody rBody;

    protected void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    // finds the corresponding visual wheel
    // correctly applies the transform

    public void FixedUpdate()
    {
        float rotateZ = speed * Input.GetAxis("Vertical");

        float rotateX = speed * Input.GetAxis("Horizontal");
        
        Vector3 target = new Vector3(rotateX, 0, rotateZ);

        rBody.AddForce(target * acceleration);


        

        // if (Input.GetKey(KeyCode.Space))
        // {
            
        // }

        //  if (rBody.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        //     {
        //         rBody.velocity *= 0.1f;
        //     }

        // velocity = rBody.velocity.sqrMagnitude;
    }
}
