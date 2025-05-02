using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float bulletSpeed;
    public AudioClip ricochetSound;
    public float maxBounces = 3;
    public GameObject explosion;

    private Rigidbody rigidBody;
    private Vector3 velocity;
    private float yPos, collisionTimer;
    private int bounceCount = 0;
    private Collider collider;


    // Use this for initialization
    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        rigidBody.centerOfMass = new Vector3(0, 0, -1);
        //fire(new Vector3Int(-1, 0, 0 ), new Vector3(1f,0,0), 3.5f);
        velocity = rigidBody.velocity;
        collider = GetComponent<Collider>();
        yPos = transform.position.y;
        //collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        collisionTimer += Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(rigidBody.velocity);
        rigidBody.velocity = ScaleVelocity(new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z), bulletSpeed);
        //transform.rotation = Quaternion.Euler(this.GetComponent<Rigidbody>().velocity);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        velocity = rigidBody.velocity;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && collisionTimer >= 0.005f)
        {
            collisionTimer = 0;
            FindObjectOfType<AudioManager>().Play("ricochetSound");
            bounceCount++;
            if (bounceCount > maxBounces)
                Destroy(this.gameObject);

            //Debug.Log("Collided");
            //Debug.Log(collision.relativeVelocity);
            //rigidBody.velocity = ScaleVelocity(rigidBody.velocity, bulletSpeed);
            // Debug-draw all contact points and normals
            velocity = Vector3.Reflect(velocity, collision.contacts[0].normal);
            rigidBody.velocity = ScaleVelocity(new Vector3(velocity.x, 0, velocity.z), bulletSpeed);
        }
    }

    /**
     * startingLocation: the location where the bullet is fired from
     * angle: the angle(from 0 to 360) that the bullet is fired at
     * speed: the speed at which the bullet is fired
     */
    void fire(Vector3 startingLocation, Vector3 angleRatio, float speed)
    {
        //use angle
        bulletSpeed = speed;
        //angle %= DEGREES_IN_CIRCLE;
        
        rigidBody.position = startingLocation; //moves to starting location
                                               //x  y  z
        rigidBody.velocity = ScaleVelocity(new Vector3(angleRatio.x, 0, angleRatio.z), bulletSpeed);
        //rigidBody.velocity = ScaleVelocity(new Vector3(1, 0, 1), speed);
        //Debug.Log(ScaleVelocity(new Vector3(1, 0, 1), speed));
        velocity = rigidBody.velocity;
        yPos = startingLocation.y;
    }

    Vector3 ScaleVelocity(Vector3 currentVelocity, float speed)
    {
        float currentMomentum = currentVelocity.magnitude;
        float scale = currentMomentum / speed;
        return currentVelocity / scale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // kill player
            other.transform.GetComponent<Death>().Damage(1);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            // kill enemy
            other.transform.GetComponent<Death>().Damage(1);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Mine"))
        {
            // explode mine
            other.gameObject.GetComponent<Mine>().Detonate();
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            GameObject.Instantiate(explosion, transform.position, new Quaternion());
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Destructible"))
        {
            /*
            GameObject.Instantiate(explosion, transform.position, new Quaternion());
            collision.gameObject.GetComponent<DestructibleWall>().Explode();
            Destroy(this.gameObject);
            */
            Physics.IgnoreCollision(collider, other.GetComponent<Collider>());
            return;
        }
    }
}
