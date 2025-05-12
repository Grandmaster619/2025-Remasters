using UnityEngine;

// ================================================================================================
// Spread bullets shoot "COUNT" shots within an "ANGLE". 
// ================================================================================================

[CreateAssetMenu(menuName = "Bullet/Spread Bullet")]
public class SpreadBullet : Bullet
{
    // Properties
    [Space]
    [Header("Spead Settings")]
    public int Count;
    public float Angle;

    // Methods
    public override void InitializeSystem(ParticleSystem bulletShootingSystem, LayerMask validTargets)
    {
        // Required setup
        base.InitializeSystem(bulletShootingSystem, validTargets);

        // Prepare the burst
        ParticleSystem.EmissionModule bsEmission = bulletShootingSystem.emission;
        ParticleSystem.ShapeModule bsShape = bulletShootingSystem.shape;

        bsEmission.enabled = true;
        bsShape.enabled = true;

        bsEmission.SetBurst(0, new ParticleSystem.Burst(0f, Count));
        bsShape.shapeType = ParticleSystemShapeType.Circle;
        bsShape.arc = Angle;
        bsShape.radiusThickness = 0;
        bsShape.sphericalDirectionAmount = 1;
        bsShape.rotation = new Vector3(90, -((180 - Angle) * 0.5f), 0);
    }

    public override void Shoot(ParticleSystem particleLauncher)
    {
        particleLauncher.Play();
    }
}