using UnityEngine;

// ================================================================================================
// Exploding bullets explode into additional particles ("EXPLOSION EFFECT") when they die or 
// collide with an enemy.
// ================================================================================================

[CreateAssetMenu(menuName = "Bullet/Explosion Bullet")]
public class ExplosionBullet : Bullet
{
    // Properties
    [Space]
    [Header("Explosion Settings")]
    public GameObject ExplosionEffect;
    public float ExplosionRange;

    // Methods
    public override void InitializeSystem(ParticleSystem particleLauncher, LayerMask validTargets)
    {
        // Required setup
        base.InitializeSystem(particleLauncher, validTargets);
    }

    public override void Shoot(ParticleSystem particleLauncher)
    {
        particleLauncher.Emit(1);
    }

    public void Explode(ParticleSystem particleLauncher)
    {
        // Spawn explosion particles at the time the bullet dies
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleLauncher.main.maxParticles];
        int count = particleLauncher.GetParticles(particles);
        
        for (int i = 0; i < count; i++)
        {
            if (particles[i].startColor.Equals((Color32)BulletColor) && particles[i].remainingLifetime <= 0.1f)
            {
                GameObject explosionObject = Instantiate(ExplosionEffect, particles[i].position, Quaternion.identity);
                explosionObject.transform.SetParent(particleLauncher.transform);
                Destroy(explosionObject, ExplosionRange);
                break;
            }
        }
    }

    public void ExplodeNow(ParticleSystem particleLauncher)
    {
        // Spawn explosion particles immediately (used when a bullet collides with an enemy)
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleLauncher.main.maxParticles];
        int count = particleLauncher.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            if (particles[i].startColor.Equals((Color32)BulletColor))
            {
                GameObject explosionObject = Instantiate(ExplosionEffect, particles[i].position, Quaternion.identity);
                explosionObject.transform.SetParent(particleLauncher.transform);
                Destroy(explosionObject, ExplosionRange);
                break;
            }
        }
    }
}