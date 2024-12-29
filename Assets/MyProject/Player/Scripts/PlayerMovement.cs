using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldownTime;
    public float airMultiplier;
    bool canJump;

    [Header("Is Grounded")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool isGrounded;
    public MoveState moveState;

    [Header("Ladder Climbing")]
    public bool isClimbing;
    public float climbSpeed = 3f;

    [Header("Buttons")]
    public KeyCode jumpButton = KeyCode.Space;
    public KeyCode sprintButton = KeyCode.LeftShift;

    [Header("Audio")]
    private AudioSource source;
    public AudioClip jumpSound;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    private void Start()
    {
        canJump = true;
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void Update()
    {
        // Check if grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        StateHandler();

        // Apply drag only on ground (and not when climbing)
        if (isGrounded && !isClimbing)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpButton) && canJump && isGrounded && !isClimbing)
        {
            canJump = false;
            source.PlayOneShot(jumpSound);
            Jump();
            Invoke(nameof(ResetJump), jumpCooldownTime);
        }
    }

    private void MovePlayer()
    {
        if (isClimbing)
        {
            // Disable gravity while climbing
            rb.useGravity = false;

            // Climbing: vertical movement only
            float climbInput = verticalInput;
            Vector3 climbDirection = new Vector3(0f, climbInput, 0f).normalized;

            // Directly set velocity along the vertical axis for climbing
            rb.velocity = new Vector3(rb.velocity.x, climbDirection.y * climbSpeed, rb.velocity.z);
        }
        else
        {
            // Enable gravity if not climbing
            rb.useGravity = true;

            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (isGrounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else
            {
                // In the air, apply less control
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }
    }

    private void SpeedControl()
    {
        // When climbing, we don't need to limit horizontal speed as strictly, 
        // but let's still ensure we don't exceed moveSpeed horizontally.
        if (isClimbing)
        {
            // Limit horizontal speed while on ladder if necessary
            Vector3 horizontalVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (horizontalVel.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = horizontalVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
            return;
        }

        // On ground or in air:
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        // Reset vertical velocity for a consistent jump
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void StateHandler()
    {
        if (isClimbing)
        {
            moveState = MoveState.Climbing;
        }
        else if (isGrounded && Input.GetKey(sprintButton))
        {
            moveState = MoveState.Sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (isGrounded)
        {
            moveState = MoveState.Walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            moveState = MoveState.InAir;
        }
    }

    public enum MoveState
    {
        Walking = 1,
        Sprinting,
        InAir,
        Climbing
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            // Stop horizontal movement when entering ladder to avoid sliding off
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
    }
}