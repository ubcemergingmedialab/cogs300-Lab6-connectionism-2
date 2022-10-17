using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ArmAgent : Agent

{
 
    public Rigidbody armRb;
    public Transform upperArm;
    public Transform Target;
    private float speed = 1; // The rotating speed
    private float RotateStep = 90f;
    private Vector3 startingPos;
    Quaternion destination;
    // Start is called before the first frame update

    void Start()
    {
        startingPos = armRb.transform.position;
    }
    void Update()
    {
        //This is just to prevent a weird bug where the arm will drift without controls
        armRb.angularVelocity = Vector3.zero;
        armRb.transform.position = startingPos;
    }


    public override void OnEpisodeBegin()
    {
        resetTarget();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //TODO: Add the observations you think you may need. 
        //Look at some of the functions below and see if you want to use some of them

        //Below are some examples of things you may (or may not) want to add
        //If you aren't sure what something does you can use Debug.Log(value) to print the value in the console
        //Feel free to add in more things if you want them
        //But the more you add, the more difficult it is for the agent to figure out what's important!

        //sensor.AddObservation(armRb.transform.position + Target.position); //Vector between two points
        //sensor.AddObservation(Target.position);
        //sensor.AddObservation(this.transform.rotation);
        //sensor.AddObservation(upperArm.position.y);
        //sensor.AddObservation(Vector3.Distance(armRb.transform.position, Target.position));


    }

    public Vector3 normalizedAngles()
    {
        Vector3 normalized = Vector3.zero;
        normalized.x = this.transform.eulerAngles.x - 360;
        normalized.y = this.transform.eulerAngles.y - 360;
        normalized.z = this.transform.eulerAngles.z;

        return normalized;
    }

    float getYAngleToTarget()
    {
        Vector3 relative = transform.InverseTransformPoint(Target.position);
        return Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
    }

    float getXAngleToTarget()
    {
        Vector3 relative = transform.InverseTransformPoint(Target.position);
        return Mathf.Atan2(relative.z, relative.y) * Mathf.Rad2Deg;
    }

    float getZAngleToTarget()
    {
        Vector3 relative = transform.InverseTransformPoint(Target.position);
        return Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
    }

    public override void OnActionReceived(ActionBuffers actions){
        Vector3 controlSignal = Vector3.zero;


        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.y = actions.ContinuousActions[1];
        controlSignal.z = actions.ContinuousActions[2];


        //TODO: add in rewards for training


        destination = armRb.transform.rotation * Quaternion.Euler(controlSignal);
        if (destination != null && armRb.transform.rotation != destination)
        {
            armRb.transform.rotation = Quaternion.RotateTowards(
                   armRb.transform.rotation,
                   destination, speed);

        }
    }
    public float rewardCloserToZero(float reward, float value)
    {
        if (value != 0)
        {
            return reward / Mathf.Abs(value);
        }

        return reward;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Target")
        {
            resetTarget();
            //TODO: What do you want to happen when you hit a target?

        }
        if (collision.collider.tag == "Ground")
        {

            resetArm();
            //TODO: What do you want to happen when you hit the ground?

            EndEpisode();
        }
    }



    //-------- IGNORE PAST THIS POINT ----------------
    public void resetTarget()
    {
        Target.localPosition = generateRandomCoordinates();
    }

    public void resetArm()
    {
        armRb.transform.rotation = Quaternion.identity;
        armRb.transform.position = startingPos;
        armRb.angularVelocity = Vector3.zero;
        armRb.velocity = Vector3.zero;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
{
     ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
    continuousActions[0] = Input.GetAxis("Horizontal");
    continuousActions[1] = Input.GetAxis("Vertical");
}
    Vector3 generateRandomCoordinates()
    {
        float anchorx = 0;
        float anchorz = 0;
        float x = anchorx + Random.value * 14 - 7;
        float y = Random.value * 9 + 1;
        float z = anchorz + Random.value * 14 - 7;

        if (x - anchorx < 3 && x - anchorx > -3 || z - anchorz < 3 && z - anchorz > -3)
        {
            return generateRandomCoordinates();
        }

        else
        {
            return new Vector3(x, y, z);
        }
    }
}