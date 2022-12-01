using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControlAPI : MonoBehaviour
{
    // Start is called before the first frame update

    PrometeoCarController controlScript;
    bool forward, backward, left, right, drift;

    void Awake(){
        controlScript = GetComponent<PrometeoCarController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       

        
        //keyboardMovement();
        raycastMovement();


        movementExecution();            
    }

    void keyboardMovement(){
      forward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
      backward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
      left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
      right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }

    void raycastMovement(){
        forward = true;
        right = false;
        left = false;

        float rightDist = Raycast(45);
        float leftDist = Raycast(-45);

        if(rightDist > leftDist){
            right = true;
        }
        else if(rightDist < leftDist){
            left = true;
        }
       
    }



    void movementExecution(){
       if(forward){
            controlScript.GoForward();
        }
        if(backward){
          controlScript.GoReverse();
        }

        if(left){
          controlScript.TurnLeft(1);
        }
        if(right){
          controlScript.TurnRight(1);
        }
        if(!(forward || backward)){
          controlScript.ThrottleOff();
        }
        if(!backward && !forward && drift){
          controlScript.reduceSpeedOverTime();
        }
        if(!left && !right){
          controlScript.ResetSteeringAngle();
        }
    }

    float Raycast(float angleOffset){

        var direction = Quaternion.Euler(0,angleOffset,0) * transform.forward;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, direction * hit.distance, Color.yellow);
            return hit.distance;
        }
        else
        {
            Debug.DrawRay(transform.position, direction * 1000, Color.red);
            return 1000f;
        }
    }
}
