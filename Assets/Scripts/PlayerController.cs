using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Speed of Player's Movement")]
    public float movementSpeed = 10;

    [Header("Required Objects")]
    [Tooltip("Transform of PlayerDirection")]
    public Transform orientation;

    [Header("Player Cameras")]
    [Tooltip("First-Person Camera")]
    public GameObject fpCam;
    [Tooltip("Top-Down Camera")]
    public GameObject tdCam;

    //[Header("Action Maps")]
    //[Tooltip("First-Person Action Map")]
    //public MapFirstPerson mapFP;
    //[Tooltip("Top-Down Action Map")]
    //public MapTopDown mapTD;

    // Array to hold our cameras, Index of current camera
    public GameObject[] cameras;
    public int activeCamera;

    // Tracks input directions
    public float moveX;
    public float moveY;

    // Direction holder and Rigidbody of player
    public Rigidbody rigidbody;

    private Vector3 movDirection;

    // Players Action Mapping
    public PlayerInput playerInput;

    [Header("Sensitivity")]
    [Tooltip("Horizontal Sens")]
    public int xSens = 400;
    [Tooltip("Vertical Sens")]
    public int ySens = 375;

    private float xRot;
    private float yRot;

    // Set up some variables before we start the code
    void Awake()
    {
        playerInput.SwitchCurrentActionMap("FirstPerson");

        // Gets the current active player camera
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
        rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(movDirection.normalized * movementSpeed, ForceMode.Force);
    }

    public void Look(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        
        // Gets our mouse position and multiplies to use deltaTime
        float mouseX = direction.x * Time.deltaTime * xSens;
        float mouseY = direction.y * Time.deltaTime * ySens;

        // Moves our rotations
        yRot += mouseX;
        xRot -= mouseY;

        // Makes it so we can only look up and down 90 degrees
        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.localRotation = Quaternion.Euler(0, yRot, 0);
        fpCam.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
    }

    public void MoveCam(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        Vector3 pos = tdCam.transform.position;
        tdCam.transform.position = new(pos.x + direction.x, 120, pos.z + direction.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            rigidbody.AddForce(Vector3.up * 6f, ForceMode.Impulse);
        }
    }

    public void SwapCam(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        cameras[activeCamera].SetActive(false);
        activeCamera = activeCamera == 0 ? 1 : 0;
        Debug.Log("Swap Camera");
        cameras[activeCamera].SetActive(true);

        if (activeCamera == 1)
        {
            playerInput.SwitchCurrentActionMap("TopCamera");
            cameras[activeCamera].transform.position = new(-80, 120, 0);
        }
        else
        {
            playerInput.SwitchCurrentActionMap("FirstPerson");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        movDirection = context.ReadValue<Vector2>();
        movDirection = orientation.right * movDirection.x + orientation.forward * movDirection.y;
    }

    // Changes the camera for the user
    // should change mapping too
    /*
    public void SwapCam(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(cameras[0].active || cameras[1].active)
            {
                // Disables our active camera
                cameras[activeCamera].SetActive(false);

                if(activeCamera == 1)
                {
                    cameras[activeCamera].transform.position = new Vector3(-80, 120, 0);
                    InputManager.playerInput.ToggleActionMap(playerInput.TopCamera);
                    // Reset the position
                }
                
                // Swaps to our next camera. If we are out of range, go back to starting camera
                activeCamera += 1;
                if(cameras.Length == activeCamera){Debug.Log(activeCamera);activeCamera = 0;}

                // Sets the new camera to active
                cameras[activeCamera].SetActive(true);
            }
        }
    }
    */
}
