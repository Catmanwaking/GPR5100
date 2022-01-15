using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviour, IPunObservable
{
    [Header("Player Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundLayer;
    [Header("PlayerOwned")]
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Camera playerCam;

    private bool isGrounded;
    private Vector2 lookInput;
    private Vector2 moveInput;
    private Vector3 camRot;
    private Vector3 playerRot;

    private Rigidbody rigidBody;
    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (!view.IsMine)
        {
            Destroy(GetComponent<PlayerInput>());
            Destroy(GetComponent<CapsuleCollider>());
            Destroy(rigidBody);
            Destroy(playerCam);
        }
        else
        {
            //add things
        }
    }

    private void Update()
    {
        if(view.IsMine)
        {
            ApplyRotation();
        }
    }

    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            isGrounded = CheckGround();
            ApplyMovement();
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

    private void ApplyRotation()
    {
        playerRot.y += lookInput.x * lookSensitivity;
        transform.rotation = Quaternion.Euler(playerRot);

        camRot.x -= lookInput.y * lookSensitivity;
        camRot.x = Mathf.Clamp(camRot.x, -85.0f, 85.0f);
        playerCam.transform.localRotation = Quaternion.Euler(camRot);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }


    #region Inputs

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void OnJump()
    {
        ApplyJump();
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
