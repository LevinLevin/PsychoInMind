using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Bewegungseinstellungen")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float sprintAcceleration = 2f; // Startboost beim Sprinten
    public float crouchSpeed = 2.5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Groundcheck")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Ducken")]
    public float crouchHeight = 1f;
    public float normalHeight = 2f;
    public float crouchTransitionSpeed = 5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isJumping;
    private bool isCrouching;
    private float currentSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        GroundCheck();
        Bewegung();
        Springen();
        Ducken();
    }

    void GroundCheck()
    {
        // Prüft, ob der Spieler den Boden berührt
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Stellt sicher, dass der Spieler den Boden hält
            isJumping = false; // Reset des Sprungs
        }
    }

    void Bewegung()
    {
        if (isGrounded) // Bewegung nur auf dem Boden zulassen
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            // Geschwindigkeit festlegen
            if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, sprintSpeed, Time.deltaTime * sprintAcceleration);
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, Time.deltaTime * sprintAcceleration);
            }

            if (isCrouching)
            {
                currentSpeed = crouchSpeed;
            }

            controller.Move(move * currentSpeed * Time.deltaTime);
        } 
    }

    void Springen()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isJumping)
        {
            isJumping = true;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Ducken()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                isCrouching = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                isCrouching = false;
            }

            // Höhe des Controllers anpassen
            float targetHeight = isCrouching ? crouchHeight : normalHeight;
            controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
        }
    }

