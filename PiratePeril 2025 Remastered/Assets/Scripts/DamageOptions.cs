using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOptions
{
    private GameObject source;
    private float force;

    public GameObject Source { get => source; }
    public float Force { get => force; }

    public DamageOptions(GameObject source)
    {
        this.source = source;
        force = 0;
    }

    public DamageOptions(GameObject source, float force)
    {
        this.source = source;
        this.force = force;
    }
}
