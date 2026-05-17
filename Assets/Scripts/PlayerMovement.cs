using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 6f;
    public float gravity = -20f;

    [Header("Rotación")]
    public LayerMask groundLayer;

    private CharacterController cc;
    private PlayerInputSystem inputActions;
    private Vector3 velocity;
    private Camera mainCam;
    private Animator animator;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        inputActions = new PlayerInputSystem();
        mainCam = Camera.main;
    }

    private void OnEnable()  => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        // Gravedad
        if (cc.isGrounded && velocity.y < 0f)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;

        cc.Move(move * moveSpeed * Time.deltaTime);
        cc.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);

        // Animaciones
        float speed = move.magnitude;
        animator.SetFloat("Speed", moveSpeed * speed, 0.1f, Time.deltaTime);
        animator.SetFloat("MotionSpeed", 1f);
        animator.SetBool("Grounded", cc.isGrounded);
    }

    private void HandleRotation()
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 lookDir = hit.point - transform.position;
            lookDir.y = 0f;

            if (lookDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, targetRot, Time.deltaTime * 20f);
            }
        }
    }

    public PlayerInputSystem Input => inputActions;
}