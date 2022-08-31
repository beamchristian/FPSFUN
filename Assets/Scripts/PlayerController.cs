using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform cameraTransform;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 1.5f;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;

    private CharacterController characterController;
    private Vector3 moveInput;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {

        Vector3 verticalMovement = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontalMovement = transform.right * Input.GetAxis("Horizontal");

        moveInput = horizontalMovement + verticalMovement;
        moveInput.Normalize();
        moveInput *= moveSpeed;

        characterController.Move(moveInput * Time.deltaTime);

        // control camera rotation
        Vector2 mouseInput = new(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y") * mouseSensitivity);

        if(invertX)
        {
            mouseInput.x = -mouseInput.x;
        }
        if(invertY)
        {
            mouseInput.y = -mouseInput.y;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        cameraTransform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));
    }
}