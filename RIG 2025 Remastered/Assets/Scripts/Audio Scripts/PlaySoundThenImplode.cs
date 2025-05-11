using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// TODO Add timer to audio splice and boolean for not using splice in separate class
public class PlaySoundThenImplode : MonoBehaviour
{
    private AudioSource src;

    void Start()
    {
        src = GetComponent<AudioSource>();
        src.Play();
    }

    public void Update()
    {
        if (!src.isPlaying)
            Destroy(gameObject);
    }
}
