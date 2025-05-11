using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour 
{
    public Transform playerBody;
    float xRotation = 0f;
    private PauseMenu pauseMenu;
    private Health health;
    public GameObject settingsObj;

    private bool isRotationLocked;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
        pauseMenu = PauseMenu.GetInstance();
        isRotationLocked = false;

        InventoryMenuController.GetInstance().OnInventoryOpened += LockRotation;
        InventoryMenuController.GetInstance().OnInventoryClosed += UnlockRotation;
    }

    void Update()
    {
        //maybe mouse sense has to go in this script and get accessed by other.
        if (!health.isAlive || pauseMenu.GetMenuState() != MenuState.None || isRotationLocked) {
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        float mouseY = Input.GetAxis("Mouse Y") * Settings.GetInstance().GetMouseSensitivity() / 3.0f;
        float mouseX = Input.GetAxis("Mouse X") * Settings.GetInstance().GetMouseSensitivity() / 3.0f;
        playerBody.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0);
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    public void LockRotation(object sender, EventArgs eventArgs)
    {
        isRotationLocked = true;
    }

    public void UnlockRotation(object sender, EventArgs eventArgs)
    {
        isRotationLocked = false;
    }
}
