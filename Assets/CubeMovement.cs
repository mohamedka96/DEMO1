using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.1f;
    [SerializeField] private float touchSensitivity = 0.1f;
    
    [Header("Camera Settings")]
    [SerializeField] private float rotationSpeed = 100f;

    private Vector3 targetPosition;
    private bool isTouchMoving = false;
    private Vector2 touchStartPosition;
    private Camera mainCamera;

    private void Start()
    {
        targetPosition = transform.position;
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found. Camera rotation will not work.");
        }
    }

    private void Update()
    {
        // Handle keyboard input on standalone platforms
        if (Application.platform != RuntimePlatform.Android && 
            Application.platform != RuntimePlatform.IPhonePlayer)
        {
            HandleKeyboardInput();
        }

        // Handle touch input on mobile platforms
        if (Input.touchCount > 0)
        {
            HandleTouchInput();
        }

        // Move the cube towards the target position
        transform.position = Vector3.MoveTowards(
            transform.position, 
            targetPosition, 
            moveSpeed * Time.deltaTime
        );
        
        // Handle camera rotation
        if (mainCamera != null)
        {
            HandleCameraRotation();
        }
    }

    private void HandleKeyboardInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Update target position based on keyboard input
            targetPosition = transform.position + new Vector3(
                horizontalInput * moveSpeed * Time.deltaTime,
                0,
                verticalInput * moveSpeed * Time.deltaTime
            );
        }
    }

    private void HandleTouchInput()
    {
        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                // Store the starting position of the touch
                touchStartPosition = touch.position;
                isTouchMoving = true;
                break;

            case TouchPhase.Moved:
                if (isTouchMoving)
                {
                    // Calculate the movement delta from the touch start
                    Vector2 touchDelta = touch.position - touchStartPosition;
                    
                    // Update target position based on touch movement
                    targetPosition = transform.position + new Vector3(
                        touchDelta.x * touchSensitivity * Time.deltaTime,
                        0,
                        touchDelta.y * touchSensitivity * Time.deltaTime
                    );
                    
                    // Update touch start position for the next frame
                    touchStartPosition = touch.position;
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                isTouchMoving = false;
                break;
        }
    }
    
    private void HandleCameraRotation()
    {
        // Rotate camera left when Q is pressed
        if (Input.GetKey(KeyCode.E))
        {
            mainCamera.transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
        
        // Rotate camera right when E is pressed
        if (Input.GetKey(KeyCode.Q))
        {
            mainCamera.transform.RotateAround(transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);
        }
    }
}