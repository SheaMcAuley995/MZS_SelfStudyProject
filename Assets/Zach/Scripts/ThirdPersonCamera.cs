using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CamData
{
    [Header("Mouse Settings")]
    public bool lockCursor;
    public float mouseSensitivity;

    [Header("Camera Settings")]
    public float distFromTarget;
    public Vector2 pitchMinMax;
    public float rotationSmoothTime; 

    [HideInInspector] public Vector3 rotationSmoothVelocity;
    [HideInInspector] public Vector3 currentRotation;

    [HideInInspector] public float yaw;
    [HideInInspector] public float pitch;
}

public class ThirdPersonCamera : MonoBehaviour
{
    public CamData cam;
    public Transform target;

	// Use this for initialization
	void Start ()
    {
        SetCameraParameters();		
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        cam.yaw += Input.GetAxis("Mouse X") * cam.mouseSensitivity;
        cam.pitch -= Input.GetAxis("Mouse Y") * cam.mouseSensitivity;
        cam.pitch = Mathf.Clamp(cam.pitch, cam.pitchMinMax.x, cam.pitchMinMax.y);

        cam.currentRotation = Vector3.SmoothDamp(cam.currentRotation, new Vector3(cam.pitch, cam.yaw), ref cam.rotationSmoothVelocity, cam.rotationSmoothTime);
        transform.eulerAngles = cam.currentRotation;

        transform.position = target.position - transform.forward * cam.distFromTarget;
	}

    void SetCameraParameters()
    {
        cam.mouseSensitivity = 10f;
        cam.distFromTarget = 2f;
        cam.pitchMinMax = new Vector2(-40, 85);
        cam.rotationSmoothTime = 0.12f;

        if (cam.lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
