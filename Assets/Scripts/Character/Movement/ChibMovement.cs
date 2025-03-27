using UnityEngine;
using Unity.Netcode;

public interface IMovement
{
    void SetMovementDirection(Vector3 direction);
    void Jump();
    void StopMovement();
    bool IsMoving();
    float GetCurrentSpeed();
    bool isGrounded { get; }
    bool isJumping { get; }
}

[RequireComponent(typeof(CharacterController))]
public class ChibMovement : NetworkBehaviour, IMovement
{
    // Movement properties
    [Header("Movement Settings")]
    [SerializeField] private float MOVE_SPEED; // 5
    [SerializeField] private float ROTATION_SPEED;  // 10
    [SerializeField] protected float JUMP_FORCE; // 5
    [SerializeField] private float GRAVITY; // 20
    
    // Movement state
    private Vector3 moveDirection = Vector3.zero; // horizontal input direction (normalized). x and z only
    private Vector3 velocity = Vector3.zero; // the actual velocity of the character
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(
        writePerm: NetworkVariableWritePermission.Owner);
    private Vector2 lastMovementDirection = Vector2.down;  // for deciding sprite when character is standing still

    // jumping
    public bool isGrounded { get; private set; }
    public bool isJumping { get; private set; }
    private float MAX_JUMP_TIME = 0.2f;
    private float jumpCounter = 0;

    // components
    private CharacterController controller;
    private IAnimated animated;

    // network
    private int updateLocationEveryNthFrame = 20;

    protected void Start()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null) Debug.LogError("CharacterController component required for CharacterMovement");

        controller.height = 0.5f;
        controller.radius = 0.08f;
    }
    
    protected void MoveCharacter()
    {
        // Calculate movement vector (horizontal only)
        Vector3 horizontalMovement = moveDirection * MOVE_SPEED * Time.fixedDeltaTime;
        
        // Combine with vertical velocity for complete movement
        Vector3 finalMovement = new Vector3(horizontalMovement.x, velocity.y * Time.fixedDeltaTime, horizontalMovement.z);
        
        // Apply movement (includes both horizontal and vertical)
        controller.Move(finalMovement);
        
        // Adjust rotation to face movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, ROTATION_SPEED * Time.fixedDeltaTime);
        }
    }

    // Modify HandleGravity to not move the character
    protected void HandleGravity()
    {   
        // Apply gravity when not grounded
        if (isGrounded) velocity.y = -0.5f; // keep character grounded
        else velocity.y -= GRAVITY * Time.fixedDeltaTime; // full gravity
    }

    // Modify FixedUpdate to ensure MoveCharacter is always called
    protected void FixedUpdate()
    {
        // Check if character is grounded
        isGrounded = controller.isGrounded;

        // Handle gravity and ground detection
        if(!isJumping) HandleGravity();
        
        // Apply movement (always, not just when horizontal movement exists)
        MoveCharacter();

        // Update network position if this is the owner and its moved
        if (IsOwner && Vector3.Distance(transform.position, Position.Value) > 0.01f)
            Position.Value = transform.position;
        
        // Animation logic
        if (moveDirection.magnitude > 0.1f && isGrounded)
        {
            // Character is moving
            Vector2 moveDir2D = new Vector2(moveDirection.x, moveDirection.z);
            lastMovementDirection = moveDir2D;
            animated.WalkInDirection(moveDir2D);
        }
        else
        {
            // Character is standing still
            if (isGrounded) animated.StandInDirection(lastMovementDirection);
            else animated.FallInDirection(lastMovementDirection);
        }
    }

    // Set the movement direction (to be called by derived classes)
    public virtual void SetMovementDirection(Vector3 direction)
    {
        // Normalize horizontal components of direction
        Vector3 horizontalDir = new Vector3(direction.x, 0, direction.z);
        
        if (horizontalDir.magnitude > 1f)
            horizontalDir.Normalize();
            
        moveDirection = horizontalDir;
    }
    
    // Attempts initiate or continue a jump
    public virtual void Jump()
    {
        if (jumpCounter < MAX_JUMP_TIME || isGrounded) 
        {
            jumpCounter = isGrounded ? 0 : jumpCounter + Time.fixedDeltaTime; // allows for holding jump key to jump higher
            isJumping = true;
            velocity.y = JUMP_FORCE;
        }
    }
    
    // Stop all movement
    public virtual void StopMovement()
    {
        moveDirection = Vector3.zero;
    }
    
    // Check if character is moving
    public bool IsMoving()
    {
        return moveDirection.magnitude > 0.1f;
    }
    
    // Get current movement speed
    public float GetCurrentSpeed()
    {
        return moveDirection.magnitude * MOVE_SPEED;
    }
}