using UnityEngine;
using Unity.Netcode;

class NetworkPlayerController : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<float> playerRotation = new NetworkVariable<float>();
    public NetworkVariable<float> cameraTilt = new NetworkVariable<float>();

    [Header("PlayerOwned")]
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Camera playerCam;

    private Vector3 camRot;
    private Vector3 playerRot;
    private float moveInputHorizontal;
    private float moveInputVertical;
    private float lookInputHorizontal;
    private float lookInputVertical;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {

        }
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        //Position.Value = 
    }

    private void Update()
    {
        transform.position = Position.Value;
    }

    private void ApplyRotation()
    {
        playerRot.y += lookInputHorizontal * lookSensitivity;
        transform.rotation = Quaternion.Euler(playerRot);

        camRot.x -= lookInputVertical * lookSensitivity;
        camRot.x = Mathf.Clamp(camRot.x, -85.0f, 85.0f);
        playerCam.transform.localRotation = Quaternion.Euler(camRot);
    }

    #region Inputs

    public void OnMoveInput(float horizontal, float vertical)
    {
        moveInputHorizontal = horizontal;
        moveInputVertical = vertical;
    }

    public void OnLookInput(float horizontal, float vertical)
    {
        lookInputHorizontal = horizontal;
        lookInputVertical = vertical;
    }

    public void OnJumpInput(float input)
    {

    }

    #endregion
}