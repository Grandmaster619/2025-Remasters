using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource openPlayerInvAudioPrefab;

    void Start()
    {
        InventoryMenuController.GetInstance().OnInventoryOpened += CreateOpenPlayerInvAudio;
    }

    public void CreateOpenPlayerInvAudio(object sender, EventArgs eventArgs)
    {
        Debug.Log("Creating OpenPlayInvAudio object");
        Instantiate(openPlayerInvAudioPrefab);
    }
}
