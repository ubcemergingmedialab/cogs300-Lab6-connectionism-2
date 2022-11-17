using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControlAPI : MonoBehaviour
{
    // Start is called before the first frame update

    PrometeoCarController controlScript;

    void Awake(){
        controlScript = GetComponent<PrometeoCarController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float rightDist = Raycast(45);
        float leftDist = Raycast(-45);




        bool forward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool backward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        forward = true;

            if(rightDist > leftDist){
                right = true;
            }
            else if(rightDist < leftDist){
                left = true;
            }


        // if(rightDist == -1){
        //     right = true;
        // }
        // else if(leftDist == -1){
        //     left = true;
        // }

        // if(Input.GetKey(KeyCode.Backspace)){
        //   rBody
        //         .AddForce(transform.forward,
        //         ForceMode.VelocityChange);
        // }

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
        if(Input.GetKey(KeyCode.Space)){
          
          controlScript.Handbrake();
        }
        if(Input.GetKeyUp(KeyCode.Space)){
          controlScript.RecoverTraction();
        }
        if(!(forward || backward)){
          controlScript.ThrottleOff();
        }
        if(!backward && !forward && !Input.GetKey(KeyCode.Space)){
          controlScript.reduceSpeedOverTime();
        }
        if(!left && !right){
          controlScript.ResetSteeringAngle();
        }
    }

    float Raycast(float angleOffset){

        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;


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
