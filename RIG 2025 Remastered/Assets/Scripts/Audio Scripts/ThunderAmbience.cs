using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAmbience : MonoBehaviour
{
    [SerializeField] private GameObject thunderAmbiencePrefab;

    [SerializeField] [Range(1,300)] private float minElapse;
    [SerializeField] [Range(1,300)] private float maxElapse;
    private float targetTime;

    void Start()
    {
        if (minElapse > maxElapse)
        {
            Debug.LogError("Ambient thunder min elapse tiem must be greater than max elapse time");
            return;
        }

        targetTime = Random.Range(minElapse, maxElapse);
    }

    void Update()
    {
        if(targetTime > 0f)
            targetTime -= Time.deltaTime;
        else
        {
            Instantiate(thunderAmbiencePrefab);
            targetTime = Random.Range(minElapse, maxElapse);
        }
    }
}
