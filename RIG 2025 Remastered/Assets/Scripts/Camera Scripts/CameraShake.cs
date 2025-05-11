using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private float shakeAmt;
    [SerializeField] private float shakeDuration;
    private Vector3 camOriginalPos;
    private float shakeTimer;
    private bool isCameraShaking;

    void Start()
    {
        camOriginalPos = transform.localPosition;
        health.OnPlayerTakeDamage += ShakeCamera;
    }

    void Update()
    {
        if(isCameraShaking && shakeTimer > 0f)
        {
            gameObject.transform.localPosition = camOriginalPos + UnityEngine.Random.insideUnitSphere * shakeAmt;

            shakeTimer -= Time.deltaTime;
        }
        else if(isCameraShaking)
        {
            isCameraShaking = false;
            gameObject.transform.localPosition = camOriginalPos;
        }
    }

    private void ShakeCamera(object sender, EventArgs eventArgs)
    {
        shakeTimer = shakeDuration;
        isCameraShaking = true;
    }
}
