using System.Collections.Generic;
using UnityEngine;

// ================================================================================================
// This component will allow its attached gameobject to recieve shots. Attach this script to any
// object you want to be able to get shot. It will only receive shots from ParticleLaunchers which
// specified its attached gameobject as a valid layer.
// ================================================================================================

public class ParticleReceiver : MonoBehaviour
{
    #region Fields

    [Header("Collision Settings")]
    [SerializeField] private GameObject collisionEffectPrefab; // Particle effect played upon collision. This can be refactored to depend on the player (or their bullet type), rather than the enemy

    private GameObject sender; // The actual object shooting the particles
    private ParticleSystem bulletSystem; // The particle system that launched the bullet that hit this gameobject

    #endregion

    private void OnParticleCollision(GameObject bulletSystemSender)
    {
        // Get the entity (Player / Enemy) who shot the bullet and its corresponding particle system and particle launcher
        bulletSystem = bulletSystemSender.GetComponent<ParticleSystem>();
        sender = bulletSystemSender.transform.parent.gameObject;
        ParticleLauncher bulletLauncher = sender.GetComponent<ParticleLauncher>(); // The component on a valid sender responsible for managing its bullets
        List<ParticleCollisionEvent> activeBulletCollisions = bulletLauncher.ActiveBulletCollisions; //The list on a valid sender storing all particle collision info

        bulletSystem.GetCollisionEvents(gameObject, activeBulletCollisions); // Get all collisions with this GameObject and the bullets
        for (int i = 0; i < activeBulletCollisions.Count; i++) // Loop through each collision and emit the corresponding effect. Should only ever be 1.
            EmitCollisionParticles(activeBulletCollisions[i]);

        // Execute any necessary events
        if (this.tag == "Enemy")
        {
            if(Random.Range(0, 99) < 5)
                Instantiate(GetComponent<EnemyController>().GetPickupPrefab(), transform.position, Quaternion.identity);
            GameObject.Find("PersistentObject").GetComponent<PersistentController>().DecEnemyCount(GetComponent<EnemyController>().GetPoints());

            transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            foreach(Collider col in GetComponents<Collider>())
                col.enabled = false;
            GetComponent<EnemyController>().enabled = false;

            Destroy(this.gameObject, 5);
        }
        else if (this.tag == "Boss")
        {
            GetComponent<BossHandler>().TakeDamage();
        }
        else if (this.tag == "Player")
            GameObject.Find("PersistentObject").GetComponent<PersistentController>().DecLives();
    }

    private void EmitCollisionParticles(ParticleCollisionEvent eventInfo)
    {
        // Set the position and rotation of the effect, then play it
        GameObject collisionEffect = Instantiate(collisionEffectPrefab, transform.position, Quaternion.identity);
        ParticleSystem collisionEffectSystem = collisionEffect.GetComponent<ParticleSystem>();
        collisionEffectSystem.Play();

        // Destroy the collisionEffect object
        Destroy(collisionEffect, collisionEffectSystem.main.duration);
    }
}