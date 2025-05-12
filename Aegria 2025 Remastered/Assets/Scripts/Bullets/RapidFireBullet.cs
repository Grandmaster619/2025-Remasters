using UnityEngine;

// ================================================================================================
// Rapid-fire bullets shoot "COUNT" shots every "DELAY" seconds.
// ================================================================================================

[CreateAssetMenu(menuName = "Bullet/Rapid-Fire Bullet")]
public class RapidFireBullet : Bullet
{
    // Properties
    [Space]
    [Header("Rapid-Fire Settings")]
    public int Count;
    public float Delay;

    // Methods
    public override void InitializeSystem(ParticleSystem bulletShootingSystem, LayerMask validTargets)
    {
        // Required setup
        base.InitializeSystem(bulletShootingSystem, validTargets);

        // Prepare the burst
        ParticleSystem.EmissionModule bsEmission = bulletShootingSystem.emission;

        bsEmission.enabled = true;
        bsEmission.SetBurst(0, new ParticleSystem.Burst(0f, 1, Count, Delay));
    }

    public override void Shoot(ParticleSystem particleLauncher)
    {
        particleLauncher.Play();
    }
}