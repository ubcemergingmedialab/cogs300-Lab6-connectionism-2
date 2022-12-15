using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DidabotController : MonoBehaviour
{

    
    float speed = 30f;
    float turnSpeed = 300f;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float frontRightDist = Raycast(45);
        float frontLeftDist = Raycast(-45);
        float backRightDist = Raycast(90);
        float backLeftDist = Raycast(-90);



        float frontRightPower = frontRightDist.Map(0,20,0.01f,1) / 1;
        float frontLeftPower = frontLeftDist.Map(0,20,0.01f,1) / 1;
        float backRightPower = backRightDist.Map(0,20,0.01f,1) / 1;
        float backLeftPower = backLeftDist.Map(0,20,0.01f,1) / 1;
        // throttlePower = frontDist.Map(0, 15, -0.3f, 1);

        HandleMovement((frontRightPower + backRightPower) - (frontLeftPower + backLeftPower));
    }


    float Raycast(float yAngleOffset){

        var direction = Quaternion.Euler(0,yAngleOffset,0) * transform.forward;
        var position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(position, direction, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(position, direction * hit.distance, Color.yellow);
            return hit.distance;
        }
        else
        {
            Debug.DrawRay(position, direction * 50, Color.red);
            return 1000f;
        }
    }


    protected virtual void HandleMovement(float rotation){
        float boundRotation = Mathf.Clamp(rotation, -1, 1);
        //Movement
        Vector3 wantedPosition = transform.position + (transform.forward * speed * Time.deltaTime);
        rb.MovePosition(wantedPosition);

        //Rotate
        Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * (boundRotation * turnSpeed * Time.deltaTime));
        rb.MoveRotation(wantedRotation);

         if(transform.position.x > 30 || transform.position.x < -30 || transform.position.z > 30 || transform.position.z < -30){
            transform.position = new Vector3(0,0,0);
        }
    }
}
