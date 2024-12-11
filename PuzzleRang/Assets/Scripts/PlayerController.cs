using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;             // Speed at which the player moves
    private bool isNotMoving = true;    // Check if player is moving, used in animations?
    private Rigidbody playerRb;         // Player's rigid body
    private Animator playerAnimator;    // Player's animator
   

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();       // Retrive player's rigid body
        playerAnimator = GetComponent<Animator>();  // Retrieve player's animator
    }

    private void Update()
    {
        if (GameManager.isGameActive){
            // Get input from WASD or Arrow keys
            float verticalInput = Input.GetAxis("Vertical");      // W/S or Up/Down arrow keys
            float horizontalInput = Input.GetAxis("Horizontal");  // A/D or Left/Right arrow keys
            
            // Create a movement vector based on input
            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
            movement = Vector3.ClampMagnitude(movement, 1);         // Vector max magnitude is equal to 1, diagonal movement is not faster than vertical/horizontal movemet

            // Move the player based on the input and speed
            transform.position += movement * moveSpeed * Time.deltaTime;

            // Check if the player is moving
            if(movement.magnitude > 0 || movement.magnitude < 0) {
                // Update Animator parameter
                playerAnimator.SetTrigger("isMoving");
                isNotMoving = false;
            }
            else isNotMoving = true;
            // Check if no movement and update animator if necessary
            playerAnimator.SetBool("isNotMoving", isNotMoving);
        }
    }
}
