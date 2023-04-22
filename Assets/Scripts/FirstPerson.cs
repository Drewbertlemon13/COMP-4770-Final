using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    // Sensitivity Variables
    [Header("Sensitivity")]
    [Tooltip("Horizontal Sensitivity")]
    public float xSens = 400;
    [Tooltip("Vertical Sensitivity")]
    public float ySens = 375;

    // The orientation of the player
    [Header("Required Objects")]
    [SerializeField]
    [Tooltip("Player Orientation")]
    public Transform orientation;

    // Variables for our rotation calculations
    private float xRot;
    private float yRot;

    // The input system for the player
    private PlayerActions playerInput;

    private Vector3 direction;

    //  Locks our cursor in the middle of the screen and makes sure it isn't visible
    //  Dave / GameDevelopment "First Person Movement in 10 Minutes" on YouTube
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerInput = new PlayerActions();
        playerInput.FirstPerson.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Look();
    }

    public void Look()
    {
        // Sets our direction to the position of our mouse
        direction = playerInput.FirstPerson.Look.ReadValue<Vector2>().normalized;
        
        // Gets our mouse position and multiplies to use deltaTime
        float mouseX = direction.x * Time.deltaTime * xSens;
        float mouseY = direction.y * Time.deltaTime * ySens;

        // Moves our rotations
        yRot += mouseX;
        xRot -= mouseY;

        // Makes it so we can only look up and down 90 degrees
        xRot = Mathf.Clamp(xRot, -90, 90);

        // Calculate our rotations
        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);
    }
}
