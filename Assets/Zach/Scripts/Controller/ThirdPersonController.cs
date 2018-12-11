using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Components and Dependencies")]
    public Animator animator;
    public Transform camera;
    public Rigidbody rb;
    Vector2 input;
    Vector2 inputDir;
    bool running;
    bool jetpackInput;
    float deltaTime;

    [Header("Movement Speeds")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float jetpackSpeed = 4f;
    float turnSmoothVelocity;
    float speedSmoothVelocity;
    float currentSpeed;

    [Header("Smoothing Settings")]
    public float turnSmoothTime = 0.2f;
    public float speedSmoothTime = 0.1f;
    public float dampTime = 0.1f;

    [Header("Jetpack Settings")]
    public float startFuel = 100f;
    public float currentFuel;
    public float fuelDrainAmount = 5f;
    float noFuel = 0f;

    [Header("Fall Damage Settings")]
    public float minFallTimeToTakeDamage = 1f;
    public float fallDamagePerSecond = 10f;
    public float deathFallTime = 3f;
    float currentFallTime;

    [Header("UI Elements")]
    public Image fuelBar;
    public Text fuelPercent;

    [Header("Extra Caching")]
    public KeyCode runKey;
    public KeyCode jetpackKey;
    public string xPosAnim = "PosX";
    public string yPosAnim = "PosY";
    Space worldSpace = Space.World;
    Space localSpace = Space.Self;
    string horizontal;
    string vertical;
    float fullAnimSpeed = 1f;
    float halfAnimSpeed = 0.5f;
    float zeroSeconds = 0f;
    float oneSecond = 1f;
    float oneHundredPercent = 100f;
    

    // Use this for initialization
    void Start ()
    {
        SetComponents();
        deltaTime = Time.deltaTime;
	}

    // Update is called once per frame
    void Update()
    {
        // For input detection
        DetectInputs();
    }

    void FixedUpdate ()
    {
        // For physical movement
        if (input != null)
        {
            ApplyMovement();
            ApplyAnimations();
        }

        if (jetpackInput && currentFuel > noFuel)
        {
            ApplyJetpack();
        }
        else
        {
            TurnOffJetpack();
        }
    }

    // Detects player inputs to apply to movement
    void DetectInputs()
    {
        input = new Vector2(Input.GetAxisRaw(horizontal), Input.GetAxisRaw(vertical));
        inputDir = input.normalized;
        running = Input.GetKey(runKey);
        jetpackInput = Input.GetKey(jetpackKey);
    }

    // Applies movement to the character based on input
    void ApplyMovement()
    {
        // If there is movement detected...
        if (inputDir != Vector2.zero)
        {
            // These two lines of code handle rotation
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        // These two lines of code handle movement speed depending on input (running or walking)
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        // Actually moves the player
        transform.Translate(transform.forward * currentSpeed * deltaTime, worldSpace);

        // Sets animation status
        float animationSpeedPercent = ((running) ? fullAnimSpeed : halfAnimSpeed) * inputDir.magnitude;
    }

    void ApplyJetpack()
    {
        currentFallTime = zeroSeconds;

        rb.useGravity = false;
        transform.Translate(transform.up * jetpackSpeed * deltaTime, worldSpace);

        currentFuel -= fuelDrainAmount * Time.deltaTime;
        HandleUI();
    }

    void TurnOffJetpack()
    {
        if (!rb.useGravity)
        {
            rb.useGravity = true;
        }

        // Begins calculating fall damage
        currentFallTime += oneSecond * Time.deltaTime;
    }

    // If the player hits something...
    void OnCollisionEnter(Collision other)
    {
        // If it's the ground...
        if (other.gameObject.layer == 15)
        {
            // If the player has fallen far enough...
            if (currentFallTime >= minFallTimeToTakeDamage && currentFallTime < deathFallTime)
            {
                // Set damage amount & take damage in PlayerHealth
                float fallDamageToTake = currentFallTime * fallDamagePerSecond;
                PlayerHealth.instance.TakeDamage(fallDamageToTake);
            }
            else if (currentFallTime >= deathFallTime)
            {
                float fallDamageToTake = PlayerHealth.instance.currentHealth;
                PlayerHealth.instance.TakeDamage(fallDamageToTake);
                PlayerHealth.instance.Die();
            }
        }
    }

    // Applies animations to the character based on movement
    void ApplyAnimations()
    {
        animator.SetFloat(xPosAnim, input.y, dampTime, deltaTime);
        animator.SetFloat(yPosAnim, input.y, dampTime, deltaTime);
    }

    // Handles the updating of jetpack fuel UI
    void HandleUI()
    {
        currentFuel = Mathf.RoundToInt(currentFuel);
        currentFuel = Mathf.Clamp(currentFuel, noFuel, startFuel);

        fuelBar.fillAmount = currentFuel / startFuel;
        fuelPercent.text = ((currentFuel / startFuel) * oneHundredPercent).ToString() + "%";
    }

    // Gets components like animator at start in case I'm a doof and forget to set them in the inspector
    void SetComponents()
    {
        horizontal = "Horizontal";
        vertical = "Vertical";

        currentFuel = startFuel;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (camera == null)
        {
            camera = Camera.main.transform;
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (runKey == null)
        {
            runKey = KeyCode.LeftShift;
        }

        if (jetpackKey == null)
        {
            jetpackKey = KeyCode.Space;
        }
    }
}
