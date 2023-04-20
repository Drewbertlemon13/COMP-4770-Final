using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class MapTopDown : PlayerController
{
    //private PlayerActions playerInput;

    // Sets up the players input
    void OnEnable()
    { 
        /*
        // Should have one already but just in case
        if(playerInput == null){
            playerInput = new PlayerActions();
        }

        playerInput.TopCamera.Enable();
        playerInput.TopCamera.SwapCamera.performed += SwapCam;
        */
    }

    // Allows us to move based on the players input
    void FixedUpdate()
    {
        //MoveCam();
    }

    // Allows us to move around in first person
    public void MoveCam()
    {
        
        //direction = playerInput.TopCamera.MoveCam.ReadValue<Vector2>().normalized;
        /*
        //direction = orientation.right * direction.x + orientation.forward * direction.y;
        // Gets our mouse position and multiplies to use deltaTime
        float mouseX = direction.x * Time.deltaTime * xSens;
        float mouseY = direction.y * Time.deltaTime * ySens;

        // Moves our rotations
        yRot += mouseX;
        xRot -= mouseY;

        // Makes it so we can only look up and down 90 degrees
        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);
        */
        //cameras[0].transform.LookAt(new Vector3(direction.x, 120, direction.y));
    }

    public void SwapCam(InputAction.CallbackContext context)
    {
        /*
        if (context.started)
        {
            if(cameras[1].active)
            {
                // Disables our active camera
                cameras[activeCamera].SetActive(false);
                
                // Swaps to our next camera. If we are out of range, go back to starting camera
                activeCamera = 0;

                // Sets the new camera to active
                cameras[activeCamera].SetActive(true);

                // Changes Mapping
                playerInput.TopCamera.Disable();
                playerInput.FirstPerson.Enable();
                Debug.Log("Swapping to FPS");
            }
        }
        */
    }

    // Unsubscribes from this TopCamera mapping so we can use another
    void OnDisable()
    {
        //playerInput.TopCamera.SwapCamera.performed -= SwapCam;
    }
}
