using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float timeRemaining = 5; // The time in seconds until the mine automatically detonates
    public float armTimeLength = 2f;
    public GameObject explosion;
    public Collider collider;

    private bool armed = false;
    private bool detonated = false;

    // Update is called once per frame
    void Update()
    {
        if (armTimeLength < 0)
        {
            armed = true;
            collider.isTrigger = false;
            // If there is time remaining, the timer decrements the time 
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            // If the time is up and the object hasn't already been destroyed, the mine detonates
            else if (this.gameObject != null)
            {
                Detonate();
            }
        }
        else
        {
            armTimeLength -= Time.deltaTime;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (armed)
        {
            // We only want it to detonate if it's hit by the player, an enemy tank, or any bullet.
            if (collision.gameObject.CompareTag("Player")
                || collision.gameObject.CompareTag("Enemy")
                || collision.gameObject.CompareTag("Bullet"))
            {
                Detonate();
            }
        }
    }

    // This method detonates the mine and deletes everything nearby it
    public void Detonate()
    {
        if (!detonated)
        {
            detonated = true;
            FindObjectOfType<AudioManager>().Play("mineExplosion");
            // creates a list of objects that exist nearby the mine using an Overlap Sphere with a radius of 2
            Collider[] objects = Physics.OverlapSphere(transform.position, 2.5f);
            // Check every object in the overlap sphere, and if the object is a player or an enemy, they get destroyed
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].gameObject.CompareTag("Player"))
                {
                    objects[i].gameObject.transform.GetComponent<Death>().Damage(int.MaxValue);
                }
                if (objects[i].gameObject.CompareTag("Enemy"))
                {
                    objects[i].gameObject.transform.GetComponent<Death>().Damage(int.MaxValue);
                }
                if (objects[i].gameObject.CompareTag("Mine"))
                {
                    objects[i].gameObject.transform.GetComponent<Mine>().Detonate();
                }
                if (objects[i].gameObject.CompareTag("Wall"))
                {
                    try
                    {
                        objects[i].gameObject.transform.GetComponent<DestructibleWall>().Explode();
                    }
                    catch
                    {
                        Debug.Log("Not a destructible wall");
                    }
                }
                // more things can be added in the future such as destroyable walls if need be

            }

            // Show an explosion animation for when the mine detonates, and then delete the mine
            GameObject.Instantiate(explosion, transform.position, new Quaternion());
            Destroy(this.gameObject);
        }
    }


}
