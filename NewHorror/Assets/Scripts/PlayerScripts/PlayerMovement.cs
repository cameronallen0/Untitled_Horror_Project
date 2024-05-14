using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls input;
    private CharacterController controller;

    public Camera cam;
    public float lookSens = 20f;

    public LayerMask groundLayer;

    private float xRot = 0f;

    private Vector3 velocity;
    private float gravity = -9.81f;
    private bool grounded;
    private float movementSpeed;
    private float walkSpeed = 3f;
    private float crouchSpeed = 1f;

    private float initHeight;
    private float crouchHeight = 1;
    private bool isCrouching;

    private void Awake()
    {
        input = new PlayerControls();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        initHeight = controller.height;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void Update()
    {
        DoMovement();
        DoLooking();
        DoCrouching();
        DoFire();
    }

    private void DoMovement()
    {
        grounded = controller.isGrounded;
        if(grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if(isCrouching && grounded)
        {
            movementSpeed = crouchSpeed;
        }
        else
        {
            movementSpeed = walkSpeed;
        }
        Vector2 movement = GetPlayerMovement();
        Vector3 move = transform.right * movement.x + transform.forward * movement.y;
        controller.Move(move * movementSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void DoLooking()
    {
        Vector2 looking = GetPlayerLook();
        float lookX = looking.x * lookSens * Time.deltaTime;
        float lookY = looking.y * lookSens * Time.deltaTime;

        xRot -= lookY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        transform.Rotate(Vector3.up * lookX);
    }

    private void DoCrouching()
    {
        if(input.Player.Crouch.ReadValue<float>() > 0)
        {
            isCrouching = true;
            controller.height = crouchHeight;
            movementSpeed = crouchSpeed;
        }
        else
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), 0.1f, -1))
            {
                controller.height = crouchHeight;
                movementSpeed = crouchSpeed;
            }
            else
            {
                controller.height = initHeight;
                movementSpeed = walkSpeed;
                isCrouching = false;

            }
        }
    }

    private void DoFire()
    {
        return;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return input.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetPlayerLook()
    {
        return input.Player.Look.ReadValue<Vector2>();
    }
}
