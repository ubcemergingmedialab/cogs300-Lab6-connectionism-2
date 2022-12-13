using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Tank_Inputs))]
public class Tank_Controller : MonoBehaviour
{
    #region Variables
    [Header("movement properties")]
    public float tankSpeed = 15f;
    public float tankRotationSpeed = 20f;

    [Header("Reticle Properties")]
    public Transform reticleTransform;

    [Header("Turret Properties")]
    public Transform turretTransform;
    public float turretTurnSpeed = 5;



    private Rigidbody rb;
    private Tank_Inputs input;
    private Vector3 finalTurretLookDir;
    #endregion


    #region Builtin Methods
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<Tank_Inputs>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rb && input){
            HandleMovement();
            HandleReticle();
            HandleTurret();
        }
    }
    #endregion


    #region Custom Methods
    protected virtual void HandleMovement(){
        
        //Movement
        Vector3 wantedPosition = transform.position + (transform.forward * input.ForwardInput *tankSpeed * Time.deltaTime);
        rb.MovePosition(wantedPosition);

        //Rotate
        Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * (tankRotationSpeed * input.RotationInput * Time.deltaTime));
        rb.MoveRotation(wantedRotation);
    }

    protected virtual void HandleReticle(){
        if(reticleTransform){
            reticleTransform.position = input.ReticlePosition;

            
        }
    }

    protected virtual void HandleTurret(){
        if(turretTransform){
            Vector3 turretLookDir = input.ReticlePosition - turretTransform.position;
            turretLookDir.y = 0f;

            finalTurretLookDir = Vector3.Lerp(finalTurretLookDir, turretLookDir, Time.deltaTime * turretTurnSpeed);
            turretTransform.rotation = Quaternion.LookRotation(finalTurretLookDir);
        }
    }
    #endregion
}
