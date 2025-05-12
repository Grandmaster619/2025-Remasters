using UnityEngine;

// ================================================================================================
// Base class for all bullets. Includes methods for preparing the shot and actually shooting, which
// can be overridden. 
// ================================================================================================

[CreateAssetMenu(menuName = "Bullet/Bullet")]
public class Bullet : ScriptableObject
{
    // Properties
    [Header("General Settings")]
    public float CooldownMultiplier;
    public float Damage;
    public float Range;
    public float Speed;
    [Range(0f, 1f)] public float TrailLength;
    public ParticleSystem.MinMaxCurve TrailWidth;
    [Space]
    public Color BulletColor;
    public Color TrailColor;
    [Space]
    public Mesh Mesh;
    public Vector3 Rotation;
    public Vector3 Scale;
    [Space]
    public AudioClip ShootSnd;

    // Methods
    public virtual void InitializeSystem(ParticleSystem bulletShootingSystem, LayerMask validTargets)
    {
        // Fields - Basically gets all the important modules used in every subclass and sets them to a default state
        ParticleSystem.MainModule bsMain = bulletShootingSystem.main;
        ParticleSystem.CollisionModule bsCollision = bulletShootingSystem.collision;
        ParticleSystem.CustomDataModule bsCustomData = bulletShootingSystem.customData;
        ParticleSystem.TrailModule bsTrail = bulletShootingSystem.trails;
        ParticleSystemRenderer bsRenderer = bulletShootingSystem.gameObject.GetComponent<ParticleSystemRenderer>();

        // Configure the particle system based on input from the current bullet's General Settings
        bsMain.duration = Range;
        bsMain.simulationSpace = ParticleSystemSimulationSpace.World;
        bsMain.startColor = BulletColor;
        bsMain.startLifetime = Range;
        bsMain.startRotation3D = true;
        bsMain.startRotationX = Rotation.x * Mathf.Deg2Rad;
        bsMain.startRotationZ = Rotation.z * Mathf.Deg2Rad;
        bsMain.startSize3D = true;
        bsMain.startSizeX = Scale.x;
        bsMain.startSizeY = Scale.y;
        bsMain.startSizeZ = Scale.z;
        bsMain.startSpeed = Speed;

        bsCollision.collidesWith = validTargets;

        bsCustomData.SetVector(ParticleSystemCustomData.Custom1, 0, Damage);

        bsTrail.colorOverTrail = TrailColor;
        bsTrail.lifetime = TrailLength;
        bsTrail.widthOverTrail = TrailWidth;

        bsRenderer.mesh = Mesh;
        
    }

    public virtual void Shoot(ParticleSystem particleLauncher)
    {
        // Actually emit the particle in the game
        SetRotation(particleLauncher);
        particleLauncher.Emit(1);
        // Debug.Log("Shot fired from " + particleLauncher.name + " of " + particleLauncher.transform.parent.name);
    }

    protected virtual void SetRotation(ParticleSystem bulletShootingSystem)
    {
        ParticleSystem.MainModule bsMain = bulletShootingSystem.main;
        bsMain.startRotationY = bulletShootingSystem.transform.parent.rotation.eulerAngles.y * Mathf.Deg2Rad;
    }
}