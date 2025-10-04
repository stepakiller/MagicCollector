using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;

    float currentSpeed;
    [Header("Передвижение")]
    [SerializeField] float Speed = 6.0f;
    [SerializeField] float sprintSpeed = 8.0f;
    [SerializeField] float crouchSpeed = 3.0f;
    [SerializeField] float crouchHeight = 1.5f;
    [SerializeField] float standingHeight = 2.0f;
    [SerializeField] float crouchSmoothTime = 0.2f; 
    
    [Header("Индикатор стамины")]
    [SerializeField] Image staminaImg;
    [SerializeField] float maxstamina;
    float currentstamina;
    [SerializeField] float durationRegen;
    [SerializeField] float durationUse;

    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -10f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;  
    bool isCrouching = false;
    bool isRunning = true;
    float jumpHeight = 3f;
    float velocityY;
    bool isGrounded;
 
    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;
    
    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;

    float targetHeight;
    float currentHeight;
    float heightVelocity;
    Vector3 targetCameraPosition;
    Vector3 cameraPositionVelocity;
 
    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        currentstamina = maxstamina;
        Application.targetFrameRate = 0;
        QualitySettings.vSyncCount = 0;

        currentHeight = standingHeight;
        targetHeight = standingHeight;
        
        targetCameraPosition = new Vector3(0, currentHeight / 2, 0);
        playerCamera.localPosition = targetCameraPosition;
    }
 
    void Update()
    {
        UpdateMouse();
        UpdateMove();
        UpdateCrouch();
        UpdateCrouchSmooth();
    }

    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
 
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
 
        cameraCap -= currentMouseDelta.y * mouseSensitivity;
 
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);
 
        playerCamera.localEulerAngles = Vector3.right * cameraCap;
 
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        Vector3 groundCheckPos = transform.position + controller.center - new Vector3(0, controller.height / 2, 0);
        isGrounded = Physics.CheckSphere(groundCheckPos, 0.2f, ground);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (isGrounded && velocityY < 0) velocityY = -2f;
        else velocityY += gravity * Time.deltaTime;

        staminaImg.fillAmount = currentstamina / maxstamina;

        currentSpeed = isCrouching ? crouchSpeed : Speed;
        if (Input.GetKey(Settings.runKey) && currentstamina > 0 && !isCrouching)
        {
            staminaImg.gameObject.transform.parent.gameObject.SetActive(true);
            isRunning = true;
            currentstamina -= durationUse;
            currentSpeed = Speed * sprintSpeed;
        }
        else isRunning = false;


        if (currentstamina <= maxstamina && !isRunning)
        {
            currentstamina += durationRegen;
            if (currentstamina >= maxstamina)
            {
                staminaImg.gameObject.transform.parent.gameObject.SetActive(false);
                currentstamina = maxstamina;
            }
        }

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            if (isCrouching && !CheckCeiling())
            {
                isCrouching = false;
                targetHeight = standingHeight;
            }
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void UpdateCrouch()
    {
        bool wantToCrouch = Input.GetKey(Settings.crouchtKey);
        
        if (wantToCrouch != isCrouching)
        {
            if (!wantToCrouch && CheckCeiling()) return;
            isCrouching = wantToCrouch;
            targetHeight = isCrouching ? crouchHeight : standingHeight;
        }

        if (isCrouching && Input.GetKey(Settings.runKey) && !CheckCeiling())
        {
            isCrouching = false;
            targetHeight = standingHeight;
        }
    }

    void UpdateCrouchSmooth()
    {
        currentHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref heightVelocity, crouchSmoothTime);
        controller.height = currentHeight;

        targetCameraPosition = new Vector3(0, currentHeight / 2, 0);
        playerCamera.localPosition = Vector3.SmoothDamp(playerCamera.localPosition, targetCameraPosition, ref cameraPositionVelocity, crouchSmoothTime);

        if (!isCrouching && currentHeight < standingHeight - 0.1f)
        {
            if (CheckCeiling())
            {
                isCrouching = true;
                targetHeight = crouchHeight;
            }
        }
    }

    bool CheckCeiling()
    {
        float rayLength = standingHeight - currentHeight + 0.1f;
        Vector3 rayStart = transform.position + Vector3.up * (currentHeight - 0.1f);
        return Physics.Raycast(rayStart, Vector3.up, rayLength, ground);
    }
}