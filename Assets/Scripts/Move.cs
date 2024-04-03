using UnityEngine;

public class Move : MonoBehaviour
{
    private CharacterController characterController;
    private float moveSpeed = 4f;
    private float rotationSpeed = 100f;
    private float gravity = 9.81f;
    private float groundCheckDistance = 0.01f; // Distance to check for ground
    private bool isGrounded; // Flag to track if the cube is grounded

    // Start is called before the first frame update
    void Start()
    {
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the cube is grounded
        isGrounded = GroundCheck();

        // Get the input axes
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction based on input
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Rotate the cube based on horizontal input
        transform.Rotate(0f, horizontalInput * rotationSpeed * Time.deltaTime, 0f);

        // Move the cube based on the input
        characterController.Move(transform.forward * verticalInput * moveSpeed * Time.deltaTime);

        // Apply gravity if the cube is not grounded
        if (!isGrounded)
        {
            characterController.Move(Vector3.down * gravity * Time.deltaTime);
        }
    }

    // Method to check if the cube is grounded
    private bool GroundCheck()
    {
        // Perform a raycast downward to check for ground
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }
}
