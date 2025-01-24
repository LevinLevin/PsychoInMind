using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Move
    [SerializeField] Transform playerCamera;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;

    //Sprint
    [Header("Move values")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float goSpeed;
    [SerializeField] private float runBuildUp;
    [SerializeField] private KeyCode runKey;

    //Jumping
    [Header("Jump values")]
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpkey;

    [SerializeField] float gravity = -13.0f;

    [Header("Smoooooth")]
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    [SerializeField] bool lockCursor = true;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;

    private CharacterController charController;

    private bool isJumping;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    //SprintMethod
    private void SprintInput()
    {
        if (Input.GetKey(runKey))
            walkSpeed = Mathf.Lerp(walkSpeed, runSpeed, Time.deltaTime * runBuildUp);
        else
            walkSpeed = Mathf.Lerp(walkSpeed, goSpeed, Time.deltaTime);
    }

    //JumpMethod
    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpkey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;
        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);

            yield return null;
        } while (!charController.isGrounded);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }

    private void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 75.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }


    //normalMovementMethod
    private void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (charController.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;


        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        charController.Move(velocity * Time.deltaTime);

        SprintInput();
        JumpInput();

    }
}
