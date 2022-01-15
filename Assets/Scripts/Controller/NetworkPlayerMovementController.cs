using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class NetworkPlayerMovementController : MonoBehaviour, IPunObservable
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
    private PhotonView view;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(!view.IsMine)
        {
            //delete things 
        }
        else
        {
            //add things
        }
    }

    private void FixedUpdate()
    {
        if(view.IsMine)
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

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheckTransform.position, groundCheckRadius);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
