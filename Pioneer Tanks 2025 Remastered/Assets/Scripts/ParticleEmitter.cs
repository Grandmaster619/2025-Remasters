using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmissionType
{
    Click,
    Collision
}

public class ParticleEmitter : MonoBehaviour
{
    // Fields
    [SerializeField] private EmissionType emissionType; // When to emit the particles
    [SerializeField] private GameObject effect; // The particle effect to play
    [SerializeField] private Transform emissionPoint; // The position to emit the particles from

    private void Update()
    {
        if (emissionType == EmissionType.Click && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(EmitParticles());
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (emissionType == EmissionType.Collision /* Check for tag eventually? */)
        {
            StartCoroutine(EmitParticles());
        }
    }

    private IEnumerator EmitParticles()
    {
        ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            float effectDuration = particleSystem.main.duration;

            GameObject particleObj = Instantiate(effect, emissionPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(effectDuration);

            Destroy(particleObj);
        }
        else
        {
            Debug.LogWarning("ParticleEmitter on " + name + " doesn't have a particle system!");
        }
    }
}