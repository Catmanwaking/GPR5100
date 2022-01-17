using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private Quaternion rotation;

    private void Start()
    {
        rotation = Quaternion.Euler(0, rotationSpeed, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation *= rotation;
    }
}
