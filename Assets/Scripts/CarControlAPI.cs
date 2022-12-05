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
        //raycastLogicMovement();
        raycastDynamicMovement();         
    }

    void keyboardMovement(){
      forward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
      backward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
      left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
      right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

      movementExecution();
    }

    void raycastLogicMovement(){
        controlScript.GoForward(1);

        float rightDist = Raycast(45);
        float leftDist = Raycast(-45);

        if(rightDist > leftDist){
            controlScript.TurnRight(1);
        }
        else if(rightDist < leftDist){
            controlScript.TurnLeft(1);
        }   
    }


    public float rightPower;
    public float leftPower;
    public float throttlePower;
    void raycastDynamicMovement(){
        

        float rightDist = Raycast(35);
        float leftDist = Raycast(-35);
        float frontDist = Raycast(0);


        rightPower = rightDist.Map(0,20,0.01f,1) / 1;
        leftPower = leftDist.Map(0,20,0.01f,1) / 1;
        throttlePower = frontDist.Map(0, 15, -0.3f, 1);

        controlScript.GoForwardBackward(throttlePower);

        controlScript.Turn((rightPower - leftPower));
    }



    void movementExecution(){
       if(forward){
            controlScript.GoForward(1);
        }
        if(backward){
          controlScript.GoReverse(1);
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
}


public static class ExtensionMethods {
 
 public static float Map (this float x, float x1, float x2, float y1,  float y2)
{
  var m = (y2 - y1) / (x2 - x1);
  var c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.
 
  return m * x + c;
}
   
}
