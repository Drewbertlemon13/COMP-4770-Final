using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    [Header("Sensitivity")]
    [Tooltip("Horizontal Sensitivity")]
    public float xSens = 400;

    [Tooltip("Vertical Sensitivity")]
    public float ySens = 375;

    [Header("Required Objects")]
    [SerializeField]
    [Tooltip("Player Orientation")]
    public Transform orientation;

    private float xRot;
    private float yRot;

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
        direction = playerInput.FirstPerson.Look.ReadValue<Vector2>().normalized;
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
    }
}
