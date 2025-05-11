using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private static Settings instance;

    [SerializeField] private float mouseSensitivity = 25;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else {
            Debug.Log("An instance Settings already exists");
            Destroy(gameObject);
        }
    }

    public float GetMouseSensitivity() { return mouseSensitivity; }

    public void SetMouseSensitivity(float mouseSensitivity) { this.mouseSensitivity = mouseSensitivity; }

    public static Settings GetInstance() { return instance; }
}
