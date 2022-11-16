using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;

    public WheelCollider rightWheel;

    public bool motor;

    public bool steering;
}

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;

    public float maxMotorTorque;

    public float maxSteeringAngle;

    public float maxSpeed;

    public float speedBoost = 50f;

    public float velocity;

    public float acceleration;

    public float motorValue;

    protected Rigidbody rBody;

    public GameObject rocketFlame;

    protected void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        rocketFlame.SetActive(false);
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");

        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space))
        {
            rocketFlame.SetActive(true);
            rBody
                .AddForce(transform.forward *= speedBoost,
                ForceMode.VelocityChange);

            if (rBody.velocity.sqrMagnitude > maxSpeed * maxSpeed * 3)
            {
                rBody.velocity *= 0.1f;
            }
        }
        else
        {
            if (rBody.velocity.sqrMagnitude > maxSpeed * maxSpeed)
            {
                rBody.velocity *= 0.1f;
            }
            rocketFlame.SetActive(false);
        }

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;

                motorValue = axleInfo.leftWheel.motorTorque;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        velocity = rBody.velocity.sqrMagnitude;
    }
}
