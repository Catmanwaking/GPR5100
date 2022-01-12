using UnityEngine;
using UnityEngine.InputSystem;

class PlayerController : MonoBehaviour
{
    [Header("PlayerOwned")]
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Camera playerCam;

    private Vector3 camRot;
    private Vector3 playerRot;
    private Vector2 lookInput;

    private void Update()
    {
        ApplyRotation();
    }

    private void ApplyRotation()
    {
        playerRot.y += lookInput.x * lookSensitivity;
        transform.rotation = Quaternion.Euler(playerRot);

        camRot.x -= lookInput.y * lookSensitivity;
        camRot.x = Mathf.Clamp(camRot.x, -85.0f, 85.0f);
        playerCam.transform.localRotation = Quaternion.Euler(camRot);
    }

    #region Inputs

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    #endregion
}