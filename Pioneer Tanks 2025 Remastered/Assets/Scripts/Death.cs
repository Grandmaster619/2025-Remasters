using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Every entity that 'lives' will need to use this death script. This way when a mine explodes or a bullet
 * hits, they are able to call the command to start the process of dying.
 */


public enum TankType
{
    PLAYER,
    BROWN,
    GREY,
    YELLOW,
    RED,
    TEAL,
    GREEN,
    BLACK
}

public class Death : MonoBehaviour
{
    public int maxHitPoints = 1;
    public GameObject explosion;
    public GameObject deadTankPrefab;
    public TankType tankType;

    private int curHitPoints;
    private PersistentController persistentController;

    private void Awake()
    {
        curHitPoints = maxHitPoints;
        persistentController = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentController>();
    }

    public void Damage(int damage)
    {
        curHitPoints -= damage;
        if(curHitPoints <= 0)
        {
            GameObject.Instantiate(explosion, transform.position, new Quaternion());
            //GameObject.Instantiate(deadTankPrefab, transform.position, new Quaternion());
            if(transform.CompareTag("Player"))
            {
                FindObjectOfType<AudioManager>().Stop("Movement");
                FindObjectOfType<AudioManager>().Play("tankExplosion");
                // do stuff to offer restarting level or going to main menu
                Destroy(this.gameObject);
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("tankExplosion");
                persistentController.AddKill(tankType);
                Destroy(this.gameObject);
            }
        }
    }

    
}
