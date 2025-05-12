using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScene2 : MonoBehaviour
{
    private static GameManagerScene2 instance;

    private bool isDrillControlPanelActive;

    private bool gasCanFilled;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        isDrillControlPanelActive = false;
    }

    public void ActivateDrillControlPanel() { isDrillControlPanelActive = true; }

    public void PlayerFilledGasCan() { gasCanFilled = true; }

    public bool IsDrillControlPanelActive() { return isDrillControlPanelActive; }

    public bool IsGasCanFilled() {  return gasCanFilled; }
    public static GameManagerScene2 GetInstance() { return instance; }
}
