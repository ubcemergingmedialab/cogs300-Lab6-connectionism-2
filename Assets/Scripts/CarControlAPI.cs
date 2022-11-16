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
        bool forward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool backward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

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
          controlScript.TurnLeft();
        }
        if(right){
          controlScript.TurnRight();
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
}
