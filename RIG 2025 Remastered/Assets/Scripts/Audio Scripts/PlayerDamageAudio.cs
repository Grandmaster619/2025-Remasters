using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageAudio : MonoBehaviour
{
    [SerializeField] private GameObject playerDamageAudioPrefab;
    [SerializeField] private Health healthScript;

    public void Start()
    {
        healthScript.OnPlayerTakeDamage += CreatePlayerDamageAudioObject;
    }

    private void CreatePlayerDamageAudioObject(object sender, EventArgs eventArgs)
    {
        Instantiate(playerDamageAudioPrefab);
    }
}
