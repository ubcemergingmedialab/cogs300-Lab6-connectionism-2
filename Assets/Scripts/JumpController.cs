using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{

    static Rigidbody rBody;

    public float force = 20f;

    float charge = 0;
    public float maxCharge = 50f;
     protected void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float rotateZ = force * Input.GetAxis("Vertical");

        float rotateX = force * Input.GetAxis("Horizontal");
        
        Vector3 target = new Vector3(rotateX, force, rotateZ);

        if (Input.GetKey(KeyCode.Space))
        {
            charge++;
            if(charge > maxCharge){
                charge = maxCharge;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space)){

            rBody.AddForce(target * charge);
            charge = 0;
        }


    }


    

}
