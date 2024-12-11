using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    public float speed = 5;         // Adjust rotation speed
    private float rotationY = 0f;   // Tracks the Y-axis rotation

    void Start()
    {
        Cursor.visible = false;                     // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked;   // Lock the cursor to the center of the screen
    }

    void Update()
    {
        // While game is running, retrieve mouse horizontal movement
        // Translate horizontal movement into rotational movement around the player's vertical/y axis
        if (GameManager.isGameActive){
            
            float mouseX = Input.GetAxis("Mouse X");    // Get mouse movement on the X-axis
            rotationY += mouseX * speed;                // Update the Y-axis rotation based on mouse movement

            transform.localRotation = Quaternion.Euler(0, rotationY, 0);    // Apply rotation to the player
        }
        else {
            Cursor.visible = true;                  // Unhide the cursor
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor from the center of the screen
        }
    }
}
