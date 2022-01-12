using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class NetworkPlayerMovementController : NetworkBehaviour
{
    [Header("Player Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;
    private Vector2 moveInput;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        isGrounded = CheckGround();
        ApplyMovement();
    }

    /// <summary>
    /// Applies the movement input of the OnMoveInput values to the player.
    /// Air movement is lerped.
    /// </summary>
    private void ApplyMovement()
    {
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        moveDirection *= speed;
        moveDirection.y += rigidBody.velocity.y;

        if (isGrounded)
            rigidBody.velocity = moveDirection;
        else
            rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, moveDirection, 2f * Time.fixedDeltaTime); //TODO magic number
    }

    /// <summary>
    /// Applies the jump input of the OnJumpInput to the player.
    /// </summary>
    private void ApplyJump()
    {
        if (isGrounded)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Performs a groundcheck using a radius around a predefined position on the player. 
    /// </summary>
    /// <returns> True if the player is touching a ground layer with his underside. </returns>
    private bool CheckGround()
    {
        return Physics.OverlapSphere(groundCheckTransform.position, groundCheckRadius, groundLayer).Length > 0;
    }

    [ServerRpc]
    public void MoveOnServerRpc(Vector2 remoteMoveInput)
    {
        moveInput = remoteMoveInput;
    }

    [ServerRpc]
    public void JumpOnServerRpc()
    {
        ApplyJump();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        MoveOnServerRpc(moveInput);
    }

    private void OnJump()
    {
        JumpOnServerRpc();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
