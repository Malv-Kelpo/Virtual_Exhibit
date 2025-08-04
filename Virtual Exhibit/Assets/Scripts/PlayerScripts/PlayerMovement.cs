using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float movementSpeed;  // Movement speed
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale; 


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Keybinds")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode showCursorKey = KeyCode.Q;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 movementDirection;

    Rigidbody rb;
    private bool isCursorLocked = true;
    
    public MovementState state;
    public enum MovementState {
        walking,
        sprinting,
        crouching
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = transform.localScale.y;
    }

    void Update()
    {
        // Check if the Escape key is pressed to show cursor
        if (Input.GetKeyDown(showCursorKey))
        {
            // Toggle the cursor lock state
            if (isCursorLocked == true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isCursorLocked = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isCursorLocked = true;
            }
        }

        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        // Movement input
        MoveInput();

        SpeedControl();

        StateHandler();

        if(grounded) {
            rb.linearDamping = groundDrag;
        }
        else {
            rb.linearDamping = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MoveInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Start Crouch
        if(Input.GetKeyDown(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // Stop Crouch
        if(Input.GetKeyUp(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
    }

    private void MovePlayer() {
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(movementDirection.normalized * movementSpeed * 2f, ForceMode.Force);
    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Limit velocity if needed
        if(flatVel.magnitude > movementSpeed) {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void StateHandler() {
        // Mode - Sprinting
        if(grounded && Input.GetKey(sprintKey)) {
            state = MovementState.sprinting;
            movementSpeed = sprintSpeed;
        }
        else if(grounded && Input.GetKey(crouchKey)) {
            state = MovementState.crouching;
            movementSpeed = crouchSpeed;
        }
        else if(grounded) {
            state = MovementState.walking;
            movementSpeed = walkSpeed;
        }
    }

    

}
