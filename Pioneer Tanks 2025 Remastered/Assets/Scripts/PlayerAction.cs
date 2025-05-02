using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAction : MonoBehaviour
{
    // Fields
    [SerializeField] public Transform emissionPoint; // The position to shoot the bullet from
    [SerializeField] public GameObject bullet;
    [SerializeField] public GameObject turret;
    [SerializeField] private float attackTimerCD = 0;
    [SerializeField] private int maxBulletStorage = 5;
    [SerializeField] private float reloadTimerCD = 2;
    [SerializeField] private GameObject effect;
    [SerializeField] private Transform particleEmissionPoint;
    [SerializeField] private int maxMines = 2;
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private GameObject mineSpawn;
    private float x, z;
    private float y = 0;
    private float speed = 6;
    private float attackTimer = 0, reloadTimer = 0, mineTimer = 0;
    private int curBullets, curMines;
    
    Rigidbody rigidBody;

    private PersistentController persistent;

    void Awake()
    {
        persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentController>();
    }

    private void Start()
    {
        curBullets = maxBulletStorage;
        curMines = maxMines;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        reloadTimer += Time.deltaTime;
        mineTimer += Time.deltaTime;

        if (!persistent.PAUSED)
        {
            if (reloadTimer >= reloadTimerCD)
            {
                curBullets = maxBulletStorage;
                reloadTimer = 0;
            }

            if (Input.GetMouseButtonDown(0) && attackTimer >= attackTimerCD && curBullets > 0)
            {
                FindObjectOfType<AudioManager>().Play("playerShot");
                attackTimer = 0;
                reloadTimer = 0;
                curBullets -= 1;
                GameObject newBullet = Instantiate(bullet, emissionPoint.transform.position, new Quaternion());
                newBullet.GetComponent<Bullet>().maxBounces = 1;
                newBullet.GetComponent<Bullet>().bulletSpeed = 6;

                rigidBody = newBullet.GetComponent<Rigidbody>();
                //rigidBody.centerOfMass = new Vector3(0, 0, -1);
                x = speed * (Mathf.Cos(turretAngleTrig(turret.transform.eulerAngles.y) * Mathf.Deg2Rad));
                z = speed * (Mathf.Sin(turretAngleTrig(turret.transform.eulerAngles.y) * Mathf.Deg2Rad));

                rigidBody.velocity = new Vector3(x, 0, z);
                StartCoroutine(EmitParticles());

                //newBullet.GetComponent<Bullet>().fire(emissionPoint.position, turret.transform.localEulerAngles, 3.5f);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (curMines > 0 && mineTimer >= 1f)
                {
                    mineTimer = 0;
                    curMines--;
                    GameObject.Instantiate(minePrefab, mineSpawn.transform.position, new Quaternion());
                }
            }

        }
    }

    float turretAngleTrig(float angle)
    {
        if (angle > 0)
            return 90 + (90 - angle);
        else
            return - 90 - (-90 + angle);
    }

    private IEnumerator EmitParticles()
    {
        ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            float effectDuration = particleSystem.main.duration;

            GameObject particleObj = Instantiate(effect, particleEmissionPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(effectDuration);

            Destroy(particleObj);
        }
        else
        {
            Debug.LogWarning("ParticleEmitter on " + name + " doesn't have a particle system!");
        }
    }
}
