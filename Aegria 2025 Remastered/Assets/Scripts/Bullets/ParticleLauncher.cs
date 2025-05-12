using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ================================================================================================
// This component will allow its attached gameobject to shoot when pressing the "Fire1" key. Attach
// this script to any object you want to be able to shoot. It should automatically load any bullet
// launchers it needs. 
// ================================================================================================

public class ParticleLauncher : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Whether or not the launcher can shoot.
    /// </summary>
    public bool CanShoot
    {
        get
        {
            return canShoot;
        }
        set
        {
            canShoot = value;
        }
    }

    /// <summary>
    /// The base time between continuous shots.
    /// </summary>
    public float ShootCooldown
    {
        get
        {
            return shootCooldown;
        }
        set
        {
            shootCooldown = value;
        }
    }

    /// <summary>
    /// The Bullet the player is currently using. Resets the cooldown if setting a new bullet.
    /// </summary>
    public Bullet CurrentBullet
    {
        get
        {
            return currentBullet;
        }
        set
        {
            currentBullet = value;

            if (shootCooldownCoroutine != null)
                StopCoroutine(shootCooldownCoroutine);
            CanShoot = true;
        }
    }

    /// <summary>
    /// The particle system currently shooting bullets.
    /// </summary>
    public ParticleSystem ActiveSystem
    {
        get
        {
            return bulletShootingSystems[CurrentBullet];
        }
    }

    #endregion

    #region Fields

    public List<ParticleCollisionEvent> ActiveBulletCollisions;

    private Coroutine shootCooldownCoroutine;
    private Dictionary<Bullet, ParticleSystem> bulletShootingSystems;

    [Header("Bullet System Settings")]
    [SerializeField] private GameObject bulletShootingSystemPrefab;
    [SerializeField] private Bullet currentBullet;
    [SerializeField] private LayerMask validTargets;

    [Header("Fire Settings")]
    [SerializeField] private bool canShoot;
    [SerializeField] private float shootCooldown;

    #endregion

    private void Awake()
    {
        // Initialize
        ActiveBulletCollisions = new List<ParticleCollisionEvent>();
        bulletShootingSystems = new Dictionary<Bullet, ParticleSystem>(BulletLibrary.Count);
    }

    private void Start()
    {
        // Initialize after all values have been defined
        CreateBulletShootingSystems();
    }

    private void CreateBulletShootingSystems()
    {
        // Create a particle system for each type of Bullet
        for (int i = 0; i < BulletLibrary.Count; i++)
        {
            // Instantiate a system and assign it a Bullet
            GameObject bulletSystemObject = Instantiate(bulletShootingSystemPrefab, transform.position, Quaternion.identity);
            Bullet bulletType = BulletLibrary.GetBullet(i);
            ParticleSystem bulletSystem = bulletSystemObject.GetComponent<ParticleSystem>();

            // Set it up and place it on the player
            bulletSystemObject.name = bulletType.name.ToUpper() + " Bullet Shooting System";
            bulletSystemObject.transform.SetParent(transform);
            bulletType.InitializeSystem(bulletSystem, validTargets);

            // Add it to the dictionary to manage all particle systems
            bulletShootingSystems.Add(bulletType, bulletSystem);
        }
    }

    private IEnumerator ActivateBullet(Action<ParticleSystem> action, float delay = 0f)
    {
        // Waits a specified amount of time
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        // Complete the action
        action(ActiveSystem);
    }

    /// <summary>
    /// Clears all bullets from the screen shot by this player.
    /// </summary>
    public void ClearAllBullets()
    {
        ActiveSystem.Clear();
    }

    /// <summary>
    /// Shoots the <paramref name="CurrentBullet"/> from the launcher.
    /// </summary>
    public void Shoot()
    {
        // Fire it off
        currentBullet.Shoot(ActiveSystem);

        // Start cooldown
        if (currentBullet is RapidFireBullet)
            shootCooldownCoroutine = StartCoroutine(StartShootCooldown(shootCooldown * currentBullet.CooldownMultiplier + (((RapidFireBullet)currentBullet).Count * ((RapidFireBullet)currentBullet).Delay)));
        else if (currentBullet is LaserBullet)
            shootCooldownCoroutine = StartCoroutine(StartShootCooldown(currentBullet.Range));
        else
            shootCooldownCoroutine = StartCoroutine(StartShootCooldown(shootCooldown * currentBullet.CooldownMultiplier));
    }

    /// <summary>
    /// Prevent the player from shooting for <paramref name="duration"/> seconds. It is a Coroutine, so remember to use StartCoroutine().
    /// </summary>
    /// <param name="duration">Time, in seconds, that the player cannot shoot for.</param>
    public IEnumerator StartShootCooldown(float duration)
    {
        CanShoot = false;

        float startTime = Time.time;
        while ((Time.time - startTime) < duration)
            yield return null;

        CanShoot = true;
        shootCooldownCoroutine = null;
    }
}