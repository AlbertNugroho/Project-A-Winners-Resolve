using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControls : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;
    public Transform playerBody;
    private FootstepManager fm;
    private CharacterController controller;
    private PsxHorrorProject inputActions;
    private Vector2 inputMove;
    private Vector2 inputLook;
    private float xRotation = 0f;
    private Vector3 velocity;
    private bool jumpQueued = false;
    private bool isRunning = false;
    private Stamina stamina;
    public float staminaUseRate = 20f;
    public float staminaRunThreshold = 10f;
    public AudioSource breath;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;
    public Animator hands;

    private void Awake()
    {
        fm = GetComponent<FootstepManager>();
        inputActions = new PsxHorrorProject();

        inputActions.Player.Move.performed += ctx => inputMove = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => inputMove = Vector2.zero;

        inputActions.Player.Look.performed += ctx => inputLook = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => inputLook = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpQueued = true;

        inputActions.Player.Run.performed += ctx => isRunning = true;
        inputActions.Player.Run.canceled += ctx => isRunning = false;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Start()
    {
        stamina = GetComponent<Stamina>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleCameraRotation();
        CheckGrounded();

        bool isMoving = inputMove.magnitude > 0.1f;

        bool canRun = isRunning && stamina.staminaBar.value >= staminaRunThreshold;

        if (isMoving && canRun)
        {
            stamina.useStamina(staminaUseRate * Time.deltaTime);
        }
        else if (isRunning && stamina.staminaBar.value < staminaRunThreshold)
        {
            breath.Play();
            isRunning = false;
            canRun = false;
        }

        HandleMovement(canRun);
        HandleFootsteps();

        if (jumpQueued)
        {
            Jump();
            jumpQueued = false;
        }

        hands.SetFloat("Speed", inputMove.magnitude * (canRun ? 2f : 1f));
        ApplyGravity();
    }



    void HandleCameraRotation()
    {
        float mouseX = inputLook.x * mouseSensitivity;
        float mouseY = inputLook.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement(bool canRun)
    {
        Vector3 moveDir = (cameraTransform.forward * inputMove.y + cameraTransform.right * inputMove.x);
        moveDir.y = 0f;
        moveDir.Normalize();

        float currentSpeed = canRun ? runSpeed : walkSpeed;
        controller.Move(moveDir * currentSpeed * Time.deltaTime);
    }


    void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
    }

    void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void HandleFootsteps()
    {
        bool isMoving = inputMove.magnitude > 0.1f && isGrounded;
        if (isMoving && !fm.Sfx.isPlaying)
            fm.PlayFootstep();
        else if (!isMoving && fm.Sfx.isPlaying)
            fm.StopFootStep();
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}
