using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;

    Transform m_Camera;
    Stamina stamina;
    public Vector3 moveVelocity;

    [SerializeField] private CharacterController controller;
    [Space]
    [SerializeField] private float walk_speed = 8f;
    [SerializeField] private float run_speed = 18f;
    [SerializeField] private float sneak_speed = 4f;
    [SerializeField] private float drained_speed = 3f;
    [SerializeField] private float drained_sneak_speed = 1f;
    [SerializeField] private float jumpHeight = 3f;
    [Space]
    [SerializeField] private float gravity = -40f;
    [Space]
    [SerializeField] private float RunningStaminaLoss = 0.18f;
    [SerializeField] private float JumpingStaminaLoss = 2.8f;
    [Space]
    [SerializeField] private float groundDistance = 2f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float maxVelocity = -30.0f;
    [Space]
    [SerializeField] private float upperEyeHeight = 1.6f;
    [SerializeField] private float lowerEyeHeight = 0f;
    [SerializeField] private float cameraHeightSpeed = 0.2f;

    private bool isSneaking = false;
    private bool isRunning = false;
    private bool isMovementLocked;
    private bool isDrained;

    private bool shiftPressed = false;
    private bool ctrlPressed = false;
    private bool jumpPressed = false;

    private float moveSpeed;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.Log("An instance of PlayerMovement already exists");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = 60;

        stamina = gameObject.GetComponent<Stamina>();
        m_Camera = gameObject.GetComponentInChildren<Camera>().transform;
        isMovementLocked = false;
        PauseMenu.GetInstance().OnPauseMenuOpened += LockMovement;
        PauseMenu.GetInstance().OnPauseMenuClosed += UnlockMovement;
        InventoryMenuController.GetInstance().OnInventoryOpened += LockMovement;
        InventoryMenuController.GetInstance().OnInventoryClosed += UnlockMovement;
    }

    void Update()
    {
        // Get Inputs =====================================
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        ctrlPressed = Input.GetKey(KeyCode.LeftControl);
        shiftPressed = Input.GetKey(KeyCode.LeftShift);
        jumpPressed = Input.GetButtonDown("Jump");
        // =================================================

        if (Time.deltaTime != 0)
        {
            if (!isMovementLocked)
            {
                if (m_Camera.localPosition.y < upperEyeHeight && (!ctrlPressed || shiftPressed))
                {
                    m_Camera.localPosition = new Vector3(0f, m_Camera.localPosition.y + cameraHeightSpeed, 0f);
                }

                isDrained = stamina.GetIsDrainedEnergyState();
                isSneaking = false;
                isRunning = false;
                if (shiftPressed && !isDrained && (x != 0 || z !=0))
                {
                    isRunning = true;
                    moveSpeed = run_speed;
                    stamina.ConsumeStamina(RunningStaminaLoss);
                }
                else if (ctrlPressed)
                {
                    if (isDrained)
                        moveSpeed = drained_sneak_speed;
                    else
                        moveSpeed = sneak_speed;
                    if (m_Camera.localPosition.y > lowerEyeHeight)
                        m_Camera.localPosition = new Vector3(0f, m_Camera.localPosition.y - cameraHeightSpeed, 0f);
                    isSneaking = true;
                }
                else if (isDrained)
                {
                    moveSpeed = drained_speed;
                }
                else
                    moveSpeed = walk_speed;

                Vector3 horizontalVelocity = (x * transform.right + z * transform.forward) * moveSpeed;
                moveVelocity = new Vector3(horizontalVelocity.x, moveVelocity.y, horizontalVelocity.z);

                if (x == 0 && z == 0)
                {
                    moveVelocity.x = 0;
                    moveVelocity.z = 0;
                }

                if (jumpPressed && IsGrounded())
                {
                    moveVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    stamina.ConsumeStamina(JumpingStaminaLoss);
                }
                else if (IsGrounded())
                    moveVelocity.y = 0;
            }
            else if (IsGrounded())
                moveVelocity.y = 0;

            moveVelocity.y += gravity * Time.deltaTime;
            if (moveVelocity.y < maxVelocity)
                moveVelocity.y = maxVelocity;

            if (IsGrounded() && ((ctrlPressed && x == 0 && z == 0) || !shiftPressed))
                stamina.RegenerateStamina(0.09f);

           
            controller.Move(moveVelocity * Time.deltaTime);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(gameObject.transform.position, -transform.up * groundDistance);
    }

    public void LockMovement(object sender, EventArgs eventArgs)
    {
        //Debug.Log("Movement Locked");
        isMovementLocked = true;
    }

    public void UnlockMovement(object sender, EventArgs eventArgs)
    {
        //Debug.Log("Movement Unlocked");
        isMovementLocked = false;
    }

    public bool IsMovementLocked() { return isMovementLocked; }

    public void SetMovementLock(bool isMovementLocked) { this.isMovementLocked = isMovementLocked; }

    public bool IsMoving() { return moveVelocity.x != 0 && moveVelocity.z != 0; }

    public bool IsGrounded() { return Physics.Raycast(gameObject.transform.position, -transform.up, groundDistance, groundMask); }

    public bool IsDrainedCrouchWalking() { return moveSpeed == drained_sneak_speed; }

    public bool IsDrained() { return isDrained; }

    public bool IsRunning() { return isRunning; }

    public bool IsSneaking() { return isSneaking; }
}
