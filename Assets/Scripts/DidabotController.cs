using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DidabotController : MonoBehaviour
{


    [Range(0, 50)]
    [SerializeField] public float speedMult;
    [Range(0, 150)]
    [SerializeField] public float turnSpeed;
    float lastSpeed;
    float lastRotation;

    [Range(0, 50)]
    [SerializeField] public float visionDistance;
    [Range(-5, 5)]
    [SerializeField] public float outMin;
    [Range(-5, 5)]
    [SerializeField] public float outMax;

    [Range(-2, 2)]
    [SerializeField] public float frontWeights;
    [Range(-2, 2)]
    [SerializeField] public float backWeights;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float inMax = visionDistance;

        float frontRightDist = Raycast(45);
        float frontLeftDist = Raycast(-45);
        float backRightDist = Raycast(90);
        float backLeftDist = Raycast(-90);


        float frontRightPower = frontRightDist.Map(0,inMax, outMin, outMax) * frontWeights;
        float frontLeftPower = frontLeftDist.Map(0, inMax, outMin, outMax) * frontWeights;
        float backRightPower = backRightDist.Map(0, inMax, outMin, outMax) * backWeights;
        float backLeftPower = backLeftDist.Map(0, inMax, outMin, outMax) * backWeights;

        float left = frontLeftPower + backLeftPower;
        float right = frontRightPower + backRightPower;


        HandleMovement(right - left, left + right);
    }


    float Raycast(float yAngleOffset){

        var direction = Quaternion.Euler(0,yAngleOffset,0) * transform.forward;
        var position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        float distance = visionDistance;
        int layerMask = 1 << 0;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(position, direction, out hit, distance, layerMask))
        {
            Debug.DrawRay(position, direction * hit.distance, Color.yellow);
            return hit.distance;
        }
        else
        {
            Debug.DrawRay(position, direction * distance, Color.red);
            return distance;
        }
    }

    protected virtual void HandleMovement(float rotation, float _speed){

        float speed = Mathf.Lerp(_speed * speedMult, lastSpeed, 0.9f);
        lastSpeed = speed;
        float boundRotation = Mathf.Clamp(rotation, -1, 1);
        //Movement
        Vector3 wantedPosition = transform.position + (transform.forward * speed * Time.deltaTime);
        rb.velocity =  transform.forward * speed;

        //Rotate
        Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * (boundRotation * turnSpeed * Time.deltaTime));
        rb.MoveRotation(wantedRotation);

         if(transform.position.x > 30 || transform.position.x < -30 || transform.position.z > 30 || transform.position.z < -30){
            transform.position = new Vector3(0,0.5f,0);
        }
    }
}
