using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravityModifier = 5f;
    [SerializeField] private float jumpPower = 5f;


    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 1.5f;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask whatIsGround;


    private bool canJump;
    private bool canDoubleJump;
    private CharacterController characterController;
    private Vector3 moveInput;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleCameraRotation();
        HandleJumping();
        HandleMovement();
    }

    private void HandleMovement()
    {
        // store y velocity
        float yStore = moveInput.y;

        Vector3 verticalMovement = transform.forward * Input.GetAxisRaw("Vertical");
        Vector3 horizontalMovement = transform.right * Input.GetAxisRaw("Horizontal");

        moveInput = horizontalMovement + verticalMovement;
        moveInput.Normalize();
        moveInput *= moveSpeed;

        moveInput.y = yStore;

        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        if (characterController.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }
    }


    private void HandleJumping()
    {
        canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;

        if(canJump)
        {
            canDoubleJump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            moveInput.y = jumpPower;

            canDoubleJump = true;
        } 
        else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            moveInput.y = jumpPower;

            canDoubleJump = false;
        }

        characterController.Move(moveInput * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        Vector2 mouseInput = new(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y") * mouseSensitivity);

        if (invertX)
        {
            mouseInput.x = -mouseInput.x;
        }
        if (invertY)
        {
            mouseInput.y = -mouseInput.y;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        cameraTransform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));
    }
}
