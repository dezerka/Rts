using UnityEngine;

public class RTSOrbitCamera : MonoBehaviour
{
    [Header("Настройки обертання")]
    public float rotationSpeed = 70f;

    [Header("Зум")]
    public float zoomSpeed = 5f;
    public float minDistance = 5f;
    public float maxDistance = 30f;

    [Header("Рух по мапі")]
    public float moveSpeed = 20f;

    [Header("Камера")]
    public Transform cameraTransform;
    public float fixedHeight = 20f;

    [Header("Кут нахилу камери")]
    [Range(10f, 89f)]
    public float tiltAngle = 30f;

    private float currentDistance = 15f;
    private float currentRotationY = 0f;

    void Start()
    {
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        UpdateCameraPosition();
    }

    void Update()
    {
        HandleRotation();
        HandleZoom();
        HandleMovement();
        UpdateCameraPosition();
    }

    void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            currentRotationY -= rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            currentRotationY += rotationSpeed * Time.deltaTime;
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance -= scroll * zoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
    }

    void HandleMovement()
    {
        Vector3 forward = Quaternion.Euler(0f, currentRotationY, 0f) * Vector3.forward;
        Vector3 right = Quaternion.Euler(0f, currentRotationY, 0f) * Vector3.right;

        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDir += forward;
        if (Input.GetKey(KeyCode.S)) moveDir -= forward;
        if (Input.GetKey(KeyCode.A)) moveDir -= right;
        if (Input.GetKey(KeyCode.D)) moveDir += right;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(tiltAngle, currentRotationY, 0f);
        Vector3 offset = rotation * new Vector3(0f, 0f, -currentDistance);

        cameraTransform.position = transform.position + offset;
        cameraTransform.LookAt(transform.position);
    }
}
