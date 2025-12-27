using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivityHorizontal = 2f;
    [SerializeField] private float mouseSensitivityVertical = 2f;
    [SerializeField] private float jumpForce = 5f;
    
    [Header("References")]
    [SerializeField] private Transform cam;
    
    [Header("Ground check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    [Header("Wall Run")] 
    [SerializeField] private float wallRunForce = 6f;
    [SerializeField] private float wallCheckDistance = 0.6f;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float camereTilt = 5f;
    [SerializeField] private float tiltSpeed = 8f;

    private float currentTilt;
    
    private bool isGrounded;
    
    private int jumpCount = 0;

    private bool isWallRunning = false;
    private bool wallOnLeft;
    private bool wallOnRight;

    float xRotation = 0f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CheckGrounded();
        CheckWall();
        WallRun();
        CamMovement();
        PlayerMove();
        PlayerJump();
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
        {
            jumpCount = 0;
        }
    }

    private void CheckWall()
    {
        wallOnRight = Physics.Raycast(transform.position, transform.right, wallCheckDistance, wallMask);
        wallOnLeft = Physics.Raycast(transform.position, -transform.right, wallCheckDistance, wallMask);
        
        if (wallOnRight || wallOnLeft)
        {
            Debug.Log("Wall running");
            jumpCount = 0;
        }
    }

    private void WallRun()
    {
        if ((wallOnLeft || wallOnRight) && !isGrounded && Input.GetAxis("Vertical") > 0.01f)
        {
            isWallRunning = true;
            rb.useGravity = false;
            
            Vector3 wallNormal = wallOnRight ? transform.right : -transform.right;
            Vector3 wallForward = Vector3.Cross(Vector3.up, wallNormal).normalized;

            rb.linearVelocity = new Vector3(
                wallForward.x * wallRunForce,
                rb.linearVelocity.y,
                wallForward.z * wallRunForce
            );
        }
        else
        {
            isWallRunning = false;
            rb.useGravity = true;
        }
    }

    void PlayerMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        rb.linearVelocity = new Vector3(move.x * moveSpeed * Time.deltaTime, rb.linearVelocity.y,
            move.z * moveSpeed * Time.deltaTime);
    }

    void CamMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityVertical * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityHorizontal * 100f * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        float targetTilt = 0f;
        if (isWallRunning)
            targetTilt = wallOnRight ? camereTilt : -camereTilt;
        
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSpeed * Time.deltaTime);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, currentTilt);
        transform.Rotate(Vector3.up * mouseX);
    }

    void PlayerJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isWallRunning)
            {
                Vector3 jumpDir = Vector3.up + (wallOnRight ? -transform.right : transform.right);
                rb.AddForce(jumpDir * jumpForce, ForceMode.Impulse);
                isWallRunning = false;
                jumpCount++;
            }
            else if (isGrounded || jumpCount < 1)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpCount++;
            }
        }
    }
}
