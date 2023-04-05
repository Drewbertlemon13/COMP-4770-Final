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

    // Tracks input directions
    private float moveX;
    private float moveY;

    private Vector3 direction;

    Rigidbody rigidbody;

    private PlayerActions playerInput;

    void Awake()
    {
        playerInput = new PlayerActions();
        playerInput.FirstPerson.Enable();
        playerInput.FirstPerson.Jump.performed += Jump;
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
}
