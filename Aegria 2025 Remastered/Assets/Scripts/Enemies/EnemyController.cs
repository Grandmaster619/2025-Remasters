using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(ParticleLauncher), typeof(AudioSource))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float spd, aggroLevel;
    [SerializeField] private int points;
    [SerializeField] private LayerMask avoidLayerType;
    [SerializeField] private GameObject collisionEffectPrefab, pickupPrefab;

    private ParticleLauncher bulletSystem;
    private float curTime, waitTime;
    private Vector3 raycastDir = Vector3.forward;
    private bool earnPoints = true;
    private PersistentController persistentController;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        bulletSystem = GetComponent<ParticleLauncher>();
        curTime = 0;
        waitTime = Random.Range((3.5f / aggroLevel), (7 / aggroLevel));
        persistentController = GameObject.FindWithTag("Persistent").GetComponent<PersistentController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
        RaycastHit hit;
        if (!(Physics.Raycast(transform.position, transform.TransformDirection(raycastDir), out hit, Mathf.Infinity, avoidLayerType)))
        {
            if (curTime >= waitTime)
            {
                audioSource.PlayOneShot(audioSource.clip);
                bulletSystem.Shoot();
                curTime = 0;
                waitTime = Random.Range((3.5f / aggroLevel), (7 / aggroLevel));
            }
            else
                curTime += Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (collisionEffectPrefab != null)
            {
                GameObject collisionEffect = Instantiate(collisionEffectPrefab, transform.position, Quaternion.identity);
                ParticleSystem collisionEffectSystem = collisionEffect.GetComponent<ParticleSystem>();
                collisionEffectSystem.Play();

                // Destroy the collisionEffect object
                Destroy(collisionEffect, collisionEffectSystem.main.duration);
            }

            Instantiate(pickupPrefab, transform.position, Quaternion.identity);
            persistentController.DecEnemyCount(0);
            Destroy(gameObject);
        }
    }

    public GameObject GetPickupPrefab()
    {
        return pickupPrefab;
    }

    public float GetSpeed()
    {
        return spd;
    }

    public float GetAggroLevel()
    {
        return aggroLevel;
    }

    public int GetPoints()
    {
        return points;
    }

    public void SetSpeed(float spd)
    {
        this.spd = spd;
        GetComponent<ParticleLauncher>().CurrentBullet.Speed = spd;
    }

    public void SetAggroLevel(float aggroLevel)
    {
        this.aggroLevel = aggroLevel;
    }

    public void SetPoints(int points)
    {
        this.points = points;
    }
}