using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement speed of our player
    [Header("Movement")]
    [Tooltip("Speed of Player's Movement")]
    public float movementSpeed = 10;

    // The orientation of our player
    [Header("Required Objects")]
    [Tooltip("Transform of PlayerDirection")]
    public Transform orientation;

    // The cameras the player can use
    [Header("Player Cameras")]
    [Tooltip("First-Person Camera")]
    public GameObject fpCam;
    [Tooltip("Top-Down Camera")]
    public GameObject tdCam;

    // Array to hold our cameras, Index of current camera
    public GameObject[] cameras;
    public int activeCamera;

    // Tracks input directions
    public float moveX;
    public float moveY;

    // Direction holder and Rigidbody of player
    public Rigidbody rg;

    // Vector to hold our direction
    private Vector3 movDirection;

    // Players Action Mapping
    public PlayerInput playerInput;

    // The sensitivity of our player
    [Header("Sensitivity")]
    [Tooltip("Horizontal Sens")]
    public int xSens = 400;
    [Tooltip("Vertical Sens")]
    public int ySens = 375;

    // Variables to calculate our rotations
    private float xRot;
    private float yRot;

    // Set up some variables before we start the code
    void Awake()
    {
        // Set the first person as our current action map
        playerInput.SwitchCurrentActionMap("FirstPerson");

        // Gets the camera in the scene
        GameObject[] tmp_cameras = {fpCam, tdCam}; // Create an array of all our cameras
        cameras = tmp_cameras;
        activeCamera = 0; // Keep track of the position of the camera in the array
        for(int i = 1; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);    // Set all other cameras to false
        }
    }

    // Gets the rigidbody of the player and freeze it's rotation so it doesn't fall over
    void Start()
    {
        rg = this.GetComponent<Rigidbody>();
        rg.freezeRotation = true;
    }

    // Move our unit based on the move direction
    void FixedUpdate()
    {
        rg.AddForce(movDirection.normalized * movementSpeed, ForceMode.Force);
    }

    // Function so we can look around
    public void Look(InputAction.CallbackContext context)
    {
        // Get the mouse position
        Vector2 direction = context.ReadValue<Vector2>();
        
        // Gets our mouse position and multiplies to use deltaTime
        float mouseX = direction.x * Time.deltaTime * xSens;
        float mouseY = direction.y * Time.deltaTime * ySens;

        // Moves our rotations
        yRot += mouseX;
        xRot -= mouseY;

        // Makes it so we can only look up and down 90 degrees
        xRot = Mathf.Clamp(xRot, -90, 90);

        // Calculates our rotation
        transform.localRotation = Quaternion.Euler(0, yRot, 0);
        fpCam.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
    }

    // Moves the third-person camera
    public void MoveCam(InputAction.CallbackContext context)
    {
        // Gets the position of the camera
        Vector2 direction = context.ReadValue<Vector2>();
        Vector3 pos = tdCam.transform.position;
        tdCam.transform.position = new(pos.x + direction.x, 120, pos.z + direction.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // Adds force to our player so we can jump up
        if(context.performed)
        {
            rg.AddForce(Vector3.up * 6f, ForceMode.Impulse);
        }
    }

    // When we hit Q(Keyboard) or Y(Controller), swap the camera
    public void SwapCam(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        // Deactivate our current camera and activate the next one
        cameras[activeCamera].SetActive(false);
        activeCamera = activeCamera == 0 ? 1 : 0;
        cameras[activeCamera].SetActive(true);

        // Change the mapping if we're in a third-person camera
        if (activeCamera == 1)
        {
            playerInput.SwitchCurrentActionMap("TopCamera");
            Cursor.lockState = CursorLockMode.None;
            
            // Recenter the camera if it moved last time
            cameras[activeCamera].transform.position = new(-80, 120, 0);
        }
        // Change the mapping if we're in a first-person view
        else
        {
            playerInput.SwitchCurrentActionMap("FirstPerson");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Allows us the move around
    public void Move(InputAction.CallbackContext context)
    {
        // Sets the move direction variable which we use in the update function to determine movement
        movDirection = context.ReadValue<Vector2>();
        movDirection = orientation.right * movDirection.x + orientation.forward * movDirection.y;
    }
}
