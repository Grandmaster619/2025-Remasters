using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundSpliceThenImplode : MonoBehaviour
{
    [SerializeField] private float spliceDuration;
    [SerializeField] private float audioStartPoint;
    private AudioSource src;
    public float audioTimer;

    void Start()
    {
        src = GetComponent<AudioSource>();
        src.time = audioStartPoint;
        src.Play();
        audioTimer = spliceDuration;
    }

    public void Update()
    {
        if (!src.isPlaying || audioTimer <= 0f)
            Destroy(gameObject);
        else
            audioTimer -= 0.05f;

/*        if (Time.deltaTime == 0)
        {
            Destroy(gameObject);
        }*/
    }
}
