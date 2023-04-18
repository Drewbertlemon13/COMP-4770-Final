using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class MapFirstPerson : PlayerController
{
    //private PlayerActions playerInput;

    // Sets up the players input
    void OnEnable()
    { 
        // Should have one already but just in case
        if(playerInput == null){
            playerInput = new PlayerActions();
        }
        
        playerInput.FirstPerson.Enable();
        playerInput.FirstPerson.Jump.performed += Jump;
        playerInput.FirstPerson.SwapCamera.performed += SwapCam;
    }

    // Allows us to move based on the players input
    void FixedUpdate()
    {
        Move();
    }

    // Allows us to jump in first person
    // Need to add the jump limit
    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            rigidbody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
        }
    }

    // Allows us to move around in first person
    public void Move()
    {
        direction = playerInput.FirstPerson.Move.ReadValue<Vector2>();
        direction = orientation.right * direction.x + orientation.forward * direction.y;
        rigidbody.AddForce(direction.normalized * movementSpeed, ForceMode.Force);
    }

    public void SwapCam(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(cameras[0].active)
            {
                // Disables our active camera
                cameras[activeCamera].SetActive(false);
                
                // Swaps to our next camera. If we are out of range, go back to starting camera
                activeCamera = 1;

                // Sets the new camera to active
                cameras[activeCamera].SetActive(true);

                // Changes Mapping
                playerInput.FirstPerson.Disable();
                playerInput.TopCamera.Enable();
                Debug.Log("Swapping to TopDown");
            }
        }
    }

    // Unsubscribes from this FPS mapping so we can use another
    void OnDisable()
    {
        playerInput.FirstPerson.Jump.performed -= Jump;
        playerInput.FirstPerson.SwapCamera.performed -= SwapCam;
    }
}
