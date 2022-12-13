using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControlAPI : MonoBehaviour
{
    // Start is called before the first frame update

    #region Variables
    PrometeoCarController controlScript;

    private float rightPower;
    private float leftPower;
    private float throttlePower;

    public bool keyboard, turing, pss, dynamic;

    bool plannedMovement = true;

    #endregion

    #region Built In
    void Awake(){
        controlScript = GetComponent<PrometeoCarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(keyboard){
          KeyboardMovement();
        }
        else if(turing){
          PrePlannedMovement();
        }
        else if (pss){
          RaycastLogicMovement();
        }
        else if (dynamic){
          RaycastDynamicMovement(); 
        }     
    }

    #endregion

    #region Movement Controls
    // ---------- PRE-PLANNED TURING ------------

    void PrePlannedMovement(){
      controlScript.GoForward(1);

      if(plannedMovement){
        double[] instructions = {0, 0, -0.5, 0, 0, 1, 0.5, 0, 0,0, -1, -0.3,0, 0.5, 0, 0, 0, -0.25, 0, 1, 0.7, 0.5, 0, 0.5, 0};
        ExectuteInstructions(instructions, 0.5);
        plannedMovement = false;
      }

       void ExectuteInstructions(double[] instructions, double delay){
          StartCoroutine(ExecuteInstructionsCoroutine(instructions, delay));

            IEnumerator ExecuteInstructionsCoroutine(double[] instructions, double delay){
              int instructionIndex = 0;
              while(instructionIndex < instructions.Length){
                float currDuration = 0;
                
                while(currDuration < delay){
                  currDuration += Time.deltaTime;
                  Debug.Log(instructions[instructionIndex]);
                  controlScript.Turn((float)instructions[instructionIndex]);
                  yield return new WaitForSeconds(0.001f);
                }
                instructionIndex++;
              }
            }
        }
    }
    //Takes in an array of instructions and exectutes them one at a time as movement commands (-1 is full left, 1 is full right, 0 is straight), waiting for delay seconds before moving to the next instruction
   
    
    
    // ------------------ LOGIC PSS ---------------

    void RaycastLogicMovement(){
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


    // ---------- DYNAMIC -------------------

    void RaycastDynamicMovement(){
        float rightDist = Raycast(35);
        float leftDist = Raycast(-35);
        float frontDist = Raycast(0);


        rightPower = rightDist.Map(0,20,0.01f,1) / 1;
        leftPower = leftDist.Map(0,20,0.01f,1) / 1;
        throttlePower = frontDist.Map(0, 15, -0.3f, 1);

        controlScript.GoForwardBackward(throttlePower);

        controlScript.Turn((rightPower - leftPower));
    }

    // ------------- KEYBOARD --------------
    void KeyboardMovement(){
      bool forward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
      bool backward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
      bool left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
      bool right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

      movementExecution(forward, backward, left, right);
      void movementExecution(bool forward, bool backward, bool left, bool right){
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
        // if(!backward && !forward && drift){
        //   controlScript.reduceSpeedOverTime();
        // }
        if(!left && !right){
          controlScript.ResetSteeringAngle();
        }
    }
    }



    #endregion

    #region Utilities


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

    #endregion
}


public static class ExtensionMethods {
 
 public static float Map (this float x, float x1, float x2, float y1,  float y2)
{
  var m = (y2 - y1) / (x2 - x1);
  var c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.
 
  return m * x + c;
}
   
}
