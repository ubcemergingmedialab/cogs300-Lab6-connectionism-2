using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Didabot3 : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody leftWheel;
    public Rigidbody rightWheel;
    [Range(0, 50)]
    [SerializeField] public float speed;
    public float left = 0;
    public float right = 0;
    [Range(-2, 2)]
    [SerializeField] public float AdjacentForwardWeight;
    [Range(-2, 2)]
    [SerializeField] public float AdjacentBackWeight;
    [Range(-2, 2)]
    [SerializeField] public float OppositeForwardWeight;
    [Range(-2, 2)]
    [SerializeField] public float OppositeBackWeight;


    void Update()
     {
        float IR0 = (1 / Raycast(-90) + 0.01f) * 100;
        float IR1 = (1 / Raycast(-45) + 0.01f) * 100;
        float IR3 = (1 / Raycast(45) + 0.01f) * 100;
        float IR4 = (1 / Raycast(90) + 0.01f) * 100;



        float leftPower = (IR0 * AdjacentBackWeight) + (IR1 * AdjacentForwardWeight) + (IR3 * OppositeForwardWeight) + (IR4 * OppositeBackWeight);

        float rightPower = (IR4 * AdjacentBackWeight) + (IR3 * AdjacentForwardWeight) + (IR1 * OppositeForwardWeight) + (IR0 * OppositeBackWeight);


        HandleMovement(leftPower, rightPower);
    }


    float Raycast(float yAngleOffset){

        var direction = Quaternion.Euler(0,yAngleOffset,0) * transform.forward;
        var position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        RaycastHit hit;
        float maxDist = 20f;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(position, direction, out hit, maxDist))
        {
            Debug.DrawRay(position, direction * hit.distance, Color.yellow);
            return hit.distance;
        }
        else
        {
            Debug.DrawRay(position, direction * maxDist, Color.red);
            return maxDist;
        }
    }

    // Update is called once per frame
    void HandleMovement(float _right, float _left)
    {
        float prevLeft = left;
        float prevRight = right;
        left = Mathf.Lerp(prevLeft, _left, 0.99999f);
        right = Mathf.Lerp(prevRight, _right, 0.99999f);
        leftWheel.velocity = (leftWheel.transform.forward * left * speed);
        rightWheel.velocity = (rightWheel.transform.forward * right * speed);
    }
}
