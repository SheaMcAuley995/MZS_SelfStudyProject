using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CamData
{
    [Header("Mouse Settings")]
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
    public CamData camData;
    public Transform target;

	// Use this for initialization
	void Start ()
    {
        SetCameraParameters();		
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        camData.yaw += Input.GetAxis("Mouse X") * camData.mouseSensitivity;
        camData.pitch -= Input.GetAxis("Mouse Y") * camData.mouseSensitivity;
        camData.pitch = Mathf.Clamp(camData.pitch, camData.pitchMinMax.x, camData.pitchMinMax.y);

        camData.currentRotation = Vector3.SmoothDamp(camData.currentRotation, new Vector3(camData.pitch, camData.yaw), ref camData.rotationSmoothVelocity, camData.rotationSmoothTime);
        transform.eulerAngles = camData.currentRotation;

        transform.position = target.position - transform.forward * camData.distFromTarget;
	}

    void SetCameraParameters()
    {
        camData.mouseSensitivity = 10f;
        camData.distFromTarget = 2f;
        camData.pitchMinMax = new Vector2(-40, 85);
        camData.rotationSmoothTime = 0.12f;

        LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
