using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PreventSoundThroughWall : MonoBehaviour
{
    private Transform player;
    private AudioSource src;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("FirstPersonPlayer").transform;
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        src.mute = true;

        LayerMask layerMask = 1;
        Vector3 direction = transform.position - player.position;
        if (Physics.Raycast(transform.position, -direction, out RaycastHit hit, 500f, ~layerMask))
        {
            //Debug.DrawRay(transform.position, -direction.normalized * hit.distance, Color.yellow, 0.1f, false); // The raycast detected something
            
            if (hit.collider.CompareTag("Player"))
            {
                src.mute = false;
            }
        }
    }
}
