using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiesAudio : MonoBehaviour
{
    [SerializeField] private GameObject playerDiesAudioPrefab;
    [SerializeField] private Health healthScript;

    public void Start()
    {
        healthScript.OnPlayerDies += CreatePlayerDiesAudioObject;
    }

    private void CreatePlayerDiesAudioObject(object sender, EventArgs eventArgs)
    {
        Instantiate(playerDiesAudioPrefab);
    }
}
