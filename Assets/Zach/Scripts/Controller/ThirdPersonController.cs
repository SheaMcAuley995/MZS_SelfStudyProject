using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Movement
{
    [Header("Movement Speeds")]
    public float walkSpeed; // 4
    public float runSpeed; // 8

    [Header("Smoothing Settings")]
    public float turnSmoothTime; // .2
    public float speedSmoothTime; // .1
    public const float dampTime = 0.1f;


    [HideInInspector] public float turnSmoothVelocity;
    [HideInInspector] public float speedSmoothVelocity;
    [HideInInspector] public float currentSpeed;
}

public class ThirdPersonController : MonoBehaviour
{
    public Movement move; 
    Animator animator;
    Transform camera;

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref move.turnSmoothVelocity, move.turnSmoothTime);
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((running) ? move.runSpeed : move.walkSpeed) * inputDir.magnitude;
        move.currentSpeed = Mathf.SmoothDamp(move.currentSpeed, targetSpeed, ref move.speedSmoothVelocity, move.speedSmoothTime);

        transform.Translate(transform.forward * move.currentSpeed * Time.deltaTime, Space.World);

        float animationSpeedPercent = ((running) ? 1 : 0.5f) * inputDir.magnitude;

        animator.SetFloat("PosX", input.y, Movement.dampTime, Time.deltaTime);
        animator.SetFloat("PosY", input.y, Movement.dampTime, Time.deltaTime);
    }
}
