using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    // Properties

    /// <summary>
    /// Whether or not the player can shoot.
    /// </summary>
    public bool CanShoot
    {
        get
        {
            return bulletSystem.CanShoot;
        }
        set
        {
            bulletSystem.CanShoot = value;
        }
    }

    // Constants
    private const float ROTATION = 30;
    [SerializeField] private const int RESTRICT_X = 60;
    [SerializeField] private const int RESTRICT_Y = 70;

    // Fields
    [SerializeField] private float speed;
    [SerializeField] private GameObject collisionEffectPrefab;
    [SerializeField] private float StartingInvincibilityTime;

    private AudioSource audioSource;
    private PersistentController PersistentInfo;
    private ParticleLauncher bulletSystem;
    private Rigidbody rb;
    private Vector3 input;
    public float invincibilityTimer;

    private void Awake()
    {
        bulletSystem = GetComponent<ParticleLauncher>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        CanShoot = true;
        invincibilityTimer = StartingInvincibilityTime;

        audioSource = GetComponent<AudioSource>();
        PersistentInfo = GameObject.Find("PersistentObject").GetComponent<PersistentController>();
    }

    private void Update()
    {
        // Shooting example
        if (CanShoot && Input.GetButton("Fire1"))
        {
            bulletSystem.Shoot();
            audioSource.PlayOneShot(audioSource.clip);
        }

        #region Unused
        // Example of how to clear bullets, this will be changed in the actual game (Power-ups, etc.)
        //if (Input.GetKeyDown(KeyCode.C))
        //    bulletSystem.ClearAllBullets();

        //Example of how to change bullets, this will be changed in the actual game(Power - ups, certain levels, etc.)
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //    BulletLibrary.PrintLibrary();

        //Bullet newBullet;
        //if (Input.GetKeyDown(KeyCode.Alpha1) && BulletLibrary.Contains("Default", out newBullet))
        //    bulletSystem.CurrentBullet = newBullet;
        //else if (Input.GetKeyDown(KeyCode.Alpha2) && BulletLibrary.Contains("Rapid", out newBullet))
        //    bulletSystem.CurrentBullet = newBullet;
        //else if (Input.GetKeyDown(KeyCode.Alpha3) && BulletLibrary.Contains("Spread", out newBullet))
        //    bulletSystem.CurrentBullet = newBullet;
        //else if (Input.GetKeyDown(KeyCode.Alpha4) && BulletLibrary.Contains("Multiple", out newBullet))
        //    bulletSystem.CurrentBullet = newBullet;
        //else if (Input.GetKeyDown(KeyCode.Alpha5) && BulletLibrary.Contains("Laser", out newBullet))
        //    bulletSystem.CurrentBullet = newBullet;

        #endregion

        // Simple movement
        input.x = Input.GetAxisRaw("Horizontal");
        input.z = Input.GetAxisRaw("Vertical");

        // Invincibility
        if (invincibilityTimer > 0f)
            invincibilityTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        #region Movement
        
        Vector3 nextPostition = transform.position;
        nextPostition.x = Mathf.Clamp(nextPostition.x + input.x * speed, -RESTRICT_X, RESTRICT_X);
        nextPostition.z = Mathf.Clamp(nextPostition.z + input.z * speed, -RESTRICT_Y, RESTRICT_Y);

        transform.rotation = Quaternion.Euler(0, 0, -ROTATION * input.x);
        transform.position = Vector3.Lerp(transform.position, nextPostition, .1f);
        
        #endregion
    }

    void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss") && GetComponent<ParticleReceiver>().enabled && invincibilityTimer < 0f)
        {
            if(collisionEffectPrefab != null)
            {
                GameObject collisionEffect = Instantiate(collisionEffectPrefab, transform.position, Quaternion.identity);
                ParticleSystem collisionEffectSystem = collisionEffect.GetComponent<ParticleSystem>();
                collisionEffectSystem.Play();

                // Destroy the collisionEffect object
                Destroy(collisionEffect, collisionEffectSystem.main.duration);
            }

            PersistentInfo.DecLives();
        }
    }
}