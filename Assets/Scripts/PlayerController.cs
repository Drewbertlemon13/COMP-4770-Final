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

    private GameObject[] cameras;

    // Tracks input directions
    private float moveX;
    private float moveY;

    private Vector3 direction;

    Rigidbody rigidbody;

    private int activeCamera;

    private PlayerActions playerInput;

    void Awake()
    {
        // Sets up the players input
        playerInput = new PlayerActions();
        playerInput.FirstPerson.Enable();
        playerInput.FirstPerson.Jump.performed += Jump;
        playerInput.FirstPerson.SwapCamera.performed += SwapCam;

        // Gets the current active player camera
        GameObject[] tmp_cameras = {fpCam, tdCam};
        cameras = tmp_cameras;
        activeCamera = 0;
        for(int i = 1; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            rigidbody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
        }
    }

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
            if(cameras[0].active || cameras[1].active)
            {
                // Disables our active camera
                cameras[activeCamera].SetActive(false);

                if(activeCamera == 1)
                {
                    cameras[activeCamera].transform.position = new Vector3(-80, 120, 0);
                    // Toggle the new map
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
}
