using UnityEngine;

// ================================================================================================
// Multiple bullets shoot "COUNT" shots at once, straight ahead. (Think 2 cannons on each wing of
// a ship)
// ================================================================================================

[CreateAssetMenu(menuName = "Bullet/Multiple-Shot Bullet")]
public class MultipleBullet : Bullet
{
    // Properties
    [Space]
    [Header("Multiple-Shot Settings")]
    public int Count;

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
        bsShape.shapeType = ParticleSystemShapeType.SingleSidedEdge;
        bsShape.sphericalDirectionAmount = 0;
        bsShape.rotation = new Vector3(90, 0, 0);
    }

    public override void Shoot(ParticleSystem particleLauncher)
    {
        particleLauncher.Play();
    }
}