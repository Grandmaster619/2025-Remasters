//using UnityEngine;

//// ================================================================================================
//// Homing bullets home on to enemies. There is a known bug, where the user activates homing bullets
//// when other types of bullets are on the screen, therefore causing these existing bullets to also 
//// begin homing. I have one idea in mind that could solve this, but am open to suggestions.
//// ================================================================================================

//[CreateAssetMenu(menuName = "Bullet/Homing Bullet")]
//public class HomingBullet : Bullet
//{
//    // Properties
//    [Space]
//    [Header("Homing Settings")]
//    public float Accuracy;
//    public float HomingSpeedMultiplier;
//    public float Sensitivity;

//    // Methods
//    protected override void PrepareForShot(ParticleSystem particleLauncher)
//    {
//        // Required setup
//        base.PrepareForShot(particleLauncher);

//        ParticleSystem.ExternalForcesModule plForces = particleLauncher.externalForces;
//        plForces.enabled = true;
//    }

//    public override void Shoot(ParticleSystem particleLauncher)
//    {
//        PrepareForShot(particleLauncher);
//        particleLauncher.Emit(1);
//    }

//    public void Home(ParticleSystem particleLauncher)
//    {
//        GameObject target = GameObject.FindGameObjectWithTag("Enemy");

//        if (target != null)
//        {
//            // Find the closest enemy
//            float minDistance = Vector3.Distance(particleLauncher.transform.position, target.transform.position);

//            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
//            {
//                if (Vector3.Distance(particleLauncher.transform.position, gameObject.transform.position) < minDistance)
//                    target = gameObject;
//            }

//            // Configure the force field with the HomingBullet's settings
//            ParticleSystem.ExternalForcesModule plForces = particleLauncher.externalForces;
//            ParticleSystemForceField targetForceField = target.GetComponent<ParticleSystemForceField>();

//            targetForceField.gravity = HomingSpeedMultiplier;
//            targetForceField.drag = Accuracy;
//            targetForceField.endRange = Sensitivity;

//            // Add it to the particle system's External Force Module to actually make this stuff work
//            plForces.SetInfluence(0, targetForceField);
//        }
//    }

//    [System.Obsolete("Use the new HomingBullet::Home method instead.")]
//    public void HomeOld(ParticleSystem particleLauncher)
//    {
//        // This was the original way I implemented homing. 
//        // It's less accurate, less configurable, looks worse, and is generally the inferior way of doing it. 
//        // Kept it for simply for a reference.

//        GameObject target = GameObject.FindGameObjectWithTag("Enemy");

//        if (target != null)
//        {
//            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleLauncher.main.maxParticles];
//            int count = particleLauncher.GetParticles(particles);

//            for (int i = 0; i < count; i++)
//            {
//                if (particles[i].startColor.Equals((Color32)BulletColor))
//                {
//                    particles[i].velocity = (target.transform.position - particles[i].position).normalized * HomingSpeedMultiplier;
//                    particles[i].rotation3D = (Quaternion.LookRotation(target.transform.position, Vector3.up).eulerAngles + new Vector3(0, 90, 0)) * Mathf.Deg2Rad;
//                }
//            }

//            particleLauncher.SetParticles(particles);
//        }
//    }
//}