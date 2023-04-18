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

    [Header("Action Maps")]
    [Tooltip("First-Person Action Map")]
    public MapFirstPerson mapFP;
    [Tooltip("Top-Down Action Map")]
    public MapTopDown mapTD;

    // Array to hold our cameras, Index of current camera
    public GameObject[] cameras;
    public int activeCamera;

    // Tracks input directions
    public float moveX;
    public float moveY;

    // Direction holder and Rigidbody of player
    public Vector3 direction;
    public Rigidbody rigidbody;

    // Players Action Mapping
    public PlayerActions playerInput;

    // Set up some variables before we start the code
    void Awake()
    {
        playerInput = new PlayerActions();
        playerInput.FirstPerson.Enable();

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
