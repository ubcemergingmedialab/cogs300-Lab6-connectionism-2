using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    public Transform body;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float scale = 1f;
        float tiltAngle = 20f;

        float tiltAroundZ = -Input.GetAxis("Horizontal") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);

        transform.position = Vector3.MoveTowards(transform.position, body.position, 0.05f);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * scale);
    }
}
