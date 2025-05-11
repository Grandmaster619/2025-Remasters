using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmfulGround : MonoBehaviour
{
    public int damage = 10;
    private Health health;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        health = player.GetComponent<Health>();
    }

    void OnTriggerStay(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Player Collided With Oil");
            health.TakeDamage(damage);
        }
    }
}
