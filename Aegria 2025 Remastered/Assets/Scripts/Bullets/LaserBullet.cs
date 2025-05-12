using UnityEngine;

// ================================================================================================
// Laser bullet shoot a constant stream of bullets, emulating a solid laser.
// ================================================================================================

[CreateAssetMenu(menuName = "Bullet/Laser Bullet")]
public class LaserBullet : Bullet
{
    // Properties
    [Space]
    [Header("Laser Settings")]
    public bool ContinuousLaser;
    public ParticleSystemSimulationSpace SimulationSpace;

    // Methods
    public override void InitializeSystem(ParticleSystem bulletShootingSystem, LayerMask validTargets)
    {
        // Required setup
        base.InitializeSystem(bulletShootingSystem, validTargets);

        // Prepare the burst
        ParticleSystem.MainModule bsMain = bulletShootingSystem.main;
        ParticleSystem.EmissionModule bsEmission = bulletShootingSystem.emission;
        ParticleSystem.TrailModule bsTrail = bulletShootingSystem.trails;

        bsMain.simulationSpace = SimulationSpace;

        bsEmission.enabled = true;
        if (ContinuousLaser)
            bsEmission.SetBurst(0, new ParticleSystem.Burst(0f, 1, 0, 0.05f)); // Pseudo-arbitrary value for a constant stream of particles, otherwise only emit 1 particle at launch.
        else
            bsEmission.SetBurst(0, new ParticleSystem.Burst(0f, 1, 0, Range));

        bsTrail.enabled = true;
    }

    public override void Shoot(ParticleSystem particleLauncher)
    {
        particleLauncher.Play();
    }
}
