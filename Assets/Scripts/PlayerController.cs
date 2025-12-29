using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeedRun = 5f;
    [SerializeField] private float moveSpeedShoot = 2f;
    [SerializeField] private float mouseSensitivityHorizontal = 2f;
    [SerializeField] private float mouseSensitivityVertical = 2f;
    [SerializeField] private float jumpForce = 5f;
    
    [Header("References")]
    [SerializeField] private Transform cam;
    [SerializeField] private Text maskUI;
    [SerializeField] private GameObject gun;
    
    [Header("Ground check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private float obstacleCheckDistance = 1f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask playerMask;

    [Header("Wall Run")] 
    [SerializeField] private float wallRunTime = 3f;
    [SerializeField] float wallJumpUpForce = 6f;
    [SerializeField] float wallJumpSideForce = 3f;
    [SerializeField] private float wallRunForce = 6f;
    [SerializeField] private float wallCheckDistance = 0.6f;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float camereTilt = 5f;
    [SerializeField] private float tiltSpeed = 8f;
    [SerializeField] private float wallRunCooldown = 1f;

    [Header("Heal")] 
    [SerializeField] private int healPerSecond = 5;

    private float currentTilt;

    private float wallRunCooldownTimer;
    
    private bool isGrounded;
    
    private int jumpCount = 0;

    private bool isWallRunning = false;
    private bool wallOnLeft;
    private bool wallOnRight;

    private float currWallRunTimer = 0f;

    float xRotation = 0f;
    Rigidbody rb;
    
    public enum Mask
    {
        Shoot,
        Run,
        Heal
    }

    private Mask currentMask;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;

        currentMask = Mask.Shoot;
        maskUI.text = "Mask: " + currentMask;
        // Debug.Log(currentMask);
    }

    void Update()
    {
        ReadScroll();
        ReadMask();
        CheckGrounded();
        CheckWall();
        WallRun();
        CamMovement();
        PlayerMove();
        PlayerJump();
        Heal();
    }

    private void ReadMask()
    {
        if (currentMask == Mask.Heal || currentMask == Mask.Run)
        {
            if (gun.activeInHierarchy)
            {
                gun.SetActive(false);
            }
        }
        else
        {
            if (!gun.activeInHierarchy)
            {
                gun.SetActive(true);
            }
        }
    }

    private void ReadScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0f) return;
        
        if (scroll > 0f)
        {
            // Debug.Log("Mouse scroll up");
            currentMask = (Mask)(((int)currentMask + 1) % 3);
        }
        else if (scroll < 0f)
        {
            // Debug.Log("Mouse scroll down");
            currentMask = (Mask)(((int)currentMask - 1 + 3) % 3);
        }
        
        maskUI.text = "Mask: " + currentMask;
        // Debug.Log(currentMask);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ~playerMask);
        if (isGrounded)
        {
            jumpCount = 0;
        }
    }

    private void CheckWall()
    {
        if (wallRunCooldownTimer == 0f)
        {
            wallOnRight = Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up,
                0.4f,
                transform.right,
                wallCheckDistance,
                wallMask
            );
            wallOnLeft = Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up,
                0.4f,
                -transform.right,
                wallCheckDistance,
                wallMask
            );
        }
        else
        { 
            wallOnLeft = false;
            wallOnRight = false;
        }

        if (wallOnRight || wallOnLeft)
        {
            // Debug.Log("Wall running");
            jumpCount = 0;
        }

        if (!wallOnLeft && !wallOnRight)
        {
            if (wallRunCooldownTimer > 0f)
            {
                wallRunCooldownTimer -= Time.deltaTime;
                if (wallRunCooldownTimer < 0f)
                    wallRunCooldownTimer = 0f;
            }
        }
    }

    private void WallRun()
    {
        if ((wallOnLeft || wallOnRight) && (currWallRunTimer < wallRunTime))
        {
            // Debug.Log("Wall Spotted");
            if (!isGrounded && Input.GetAxis("Vertical") > 0.01f)
            {
                if (!isWallRunning)
                {
                    currWallRunTimer = 0f;
                }
                currWallRunTimer += Time.deltaTime;
                isWallRunning = true;
                rb.useGravity = false;

                Vector3 wallNormal = wallOnRight ? -transform.right : transform.right;
                Vector3 wallForward = Vector3.ProjectOnPlane(transform.forward, wallNormal).normalized;
                
                float input = Input.GetAxis("Vertical");
                Vector3 moveAlongWall = wallForward * input * wallRunForce;

                rb.linearVelocity = new Vector3(
                    moveAlongWall.x,
                    rb.linearVelocity.y,
                    moveAlongWall.z
                );

                if (currWallRunTimer >= wallRunTime)
                {
                    Invoke(nameof(ResetCurrentWallRunTime), 2f);
                }
            }
            else
            {
                // Debug.Log("Wall there but not wall running");
                isWallRunning = false;
                rb.useGravity = true;
            }
        }
        else
        {
            isWallRunning = false;
            rb.useGravity = true;
        }
    }

    void PlayerMove()
    {
        if (!isWallRunning)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            
            Vector3 moveDir = move.normalized;
            if (Physics.Raycast(transform.position, moveDir, obstacleCheckDistance))
            {
                // Debug.Log("Something in way");
                return;
            }

            if (currentMask == Mask.Run)
            {
                rb.linearVelocity = new Vector3(move.x * moveSpeedRun, rb.linearVelocity.y,
                    move.z * moveSpeedRun);
            }
            else if (currentMask == Mask.Shoot)
            {
                rb.linearVelocity = new Vector3(move.x * moveSpeedShoot, rb.linearVelocity.y,
                    move.z * moveSpeedShoot);
            }
        }
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
                Vector3 sideDir = wallOnRight ? -transform.right : transform.right;

                Vector3 jumpForceVec =
                    Vector3.up * wallJumpUpForce +
                    sideDir * wallJumpSideForce;

                rb.AddForce(jumpForceVec, ForceMode.Impulse);

                isWallRunning = false;
                wallRunCooldownTimer = wallRunCooldown;
                
                jumpCount++;
            }
            else if (isGrounded || jumpCount < 1)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpCount++;
            }
        }
    }

    private void Heal()
    {
        if (currentMask == Mask.Heal)
        {
            gameObject.GetComponent<PlayerHealth>().Heal(healPerSecond * Time.deltaTime);
        }
    }

    public Mask GetCurrentMask()
    {
        return currentMask;
    }

    private void ResetCurrentWallRunTime()
    {
        currWallRunTimer = 0f;
    }
}
