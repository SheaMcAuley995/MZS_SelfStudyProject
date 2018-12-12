using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Wheel Components")]
    public WheelCollider frontDriverW;
    public WheelCollider frontPassengerW;
    public WheelCollider rearDriverW;
    public WheelCollider rearPassengerW;
    public Transform frontDriverT;
    public Transform frontPassengerT;
    public Transform rearDriverT;
    public Transform rearPassengerT;

    [Header("Settings")]
    public float maxSteerAngle = 30f; // How much we can turn
    public float motorForce = 50f; // How fast we can go

    float horizontalInput;
    float verticalInput;
    float steeringAngle;

	
	void FixedUpdate()
    {
        GetInput();

        if (horizontalInput != 0)
        {
            Steer();
        }
        if (verticalInput != 0)
        {
            Accelerate();
        }

        UpdateWheelPoses();
	}

    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalInput;
        frontDriverW.steerAngle = steeringAngle;
        frontPassengerW.steerAngle = steeringAngle;
    }

    void Accelerate()
    {
        frontDriverW.motorTorque = -verticalInput * motorForce;
        frontPassengerW.motorTorque = -verticalInput * motorForce;
    }

    void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 pos = _transform.position;
        Quaternion rot = _transform.rotation;

        _collider.GetWorldPose(out pos, out rot);

        _transform.position = pos;
        _transform.rotation = rot;
    }
}
