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
    [HideInInspector] public Vector2 input;
    [HideInInspector] public Vector2 inputDir;
    [HideInInspector] public bool running;
}

public class ThirdPersonController : MonoBehaviour
{
    public Movement move; 
    Animator animator;
    Transform camera;
    float deltaTime;

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main.transform;
        deltaTime = Time.deltaTime;
	}

    void Update()
    {
        DetectInputs();
    }

    void FixedUpdate ()
    {
        ApplyMovement();
        ApplyAnimations();
    }

    void DetectInputs()
    {
        move.input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        move.inputDir = move.input.normalized;
        move.running = Input.GetKey(KeyCode.LeftShift);
    }

    void ApplyMovement()
    {
        if (move.inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(move.inputDir.x, move.inputDir.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref move.turnSmoothVelocity, move.turnSmoothTime);
        }

        float targetSpeed = ((move.running) ? move.runSpeed : move.walkSpeed) * move.inputDir.magnitude;
        move.currentSpeed = Mathf.SmoothDamp(move.currentSpeed, targetSpeed, ref move.speedSmoothVelocity, move.speedSmoothTime);

        transform.Translate(transform.forward * move.currentSpeed * deltaTime, Space.World);

        float animationSpeedPercent = ((move.running) ? 1 : 0.5f) * move.inputDir.magnitude;
    }

    void ApplyAnimations()
    {
        animator.SetFloat("PosX", move.input.y, Movement.dampTime, deltaTime);
        animator.SetFloat("PosY", move.input.y, Movement.dampTime, deltaTime);
    }
}
