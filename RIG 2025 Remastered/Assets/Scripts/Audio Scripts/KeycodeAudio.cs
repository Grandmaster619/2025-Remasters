using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycodeAudio : MonoBehaviour
{
    [SerializeField] private PassCheck passCheck;
    public AudioSource keyPress;
    public AudioSource keycodeCorrect;

    public EventHandler<EventArgs> OnKeyPress;

    public void Start()
    {
        passCheck.OnKeycodeCorrect += PassKeyCorrectCode;
    }

    private void PassKeyCorrectCode(object sender, EventArgs eventargs)
    {
        keycodeCorrect.Play();
    }

    public void KeyPressed()
    {
        keyPress.Play();
    }
}