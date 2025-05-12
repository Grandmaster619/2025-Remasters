using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandler : MonoBehaviour {
    private Rigidbody rb;
    private Transform tnf;

    private Transform target;

    private Transform bulletSpawn;
    [SerializeField] private Bullet bullet, beam, spread;
    private ParticleLauncher bulletSystem;
    private AudioSource audioSource;

    public int phase = 1;
    public float speed = 30f;
    public static int maxHealth = 1000;
    public int health = maxHealth;
    private float burstRate = 1.0f;
    private int burstCount = 5;
    private float fireSpeed = 0.2f;
    private float lastBurst;
    private float lastFired;
    private float startTime;
    private int petals, points;
    private bool moveInPattern = false;

    public float horizontalBound, lowerVerticalBound, upperVerticalBound;

    // Use this for initialization
    void Start () {
        rb = this.gameObject.GetComponent<Rigidbody>();
        tnf = this.transform;
        bulletSystem = GetComponent<ParticleLauncher>();

        lastBurst = 0.0f;
        lastFired = 0.0f;

        audioSource = GetComponent<AudioSource>();
        target = GameObject.Find("Player").transform;
        startTime = Time.time;

        petals = Random.Range(2, 10);
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
        moveBoss();
        boundMovement();
        bossShoot();
        phaseCheck();
    }

    void bossShoot()
    {
        if(GameObject.Find("PersistentObject").GetComponent<PersistentController>().GetLives() > 0)
            target = GameObject.Find("Player").transform;

        if (phase == 1)
        {
            bulletSystem.CurrentBullet = bullet;
            if(lastBurst + burstRate <= Time.time)
            {
                if (lastFired + fireSpeed <= Time.time)
                {
                    audioSource.PlayOneShot(audioSource.clip);
                    bulletSystem.Shoot();
                    lastFired = Time.time;
                    if(lastBurst + burstRate + (fireSpeed * burstCount) < Time.time)
                    {
                        lastBurst = Time.time;
                    }
                }
                
            }
        }
        else if (phase == 2)
        {
            bulletSystem.CurrentBullet = beam;
            if (lastFired + fireSpeed * 8 <= Time.time)
            {
                lastFired = Time.time;
                audioSource.PlayOneShot(audioSource.clip);
                bulletSystem.Shoot();
            }
        }
        else if (phase == 3)
        {
            if(moveInPattern)
            {
                bulletSystem.CurrentBullet = spread;
                tnf.LookAt(target);
                if (lastFired + fireSpeed * 10 <= Time.time)
                {
                    lastFired = Time.time;
                    audioSource.PlayOneShot(audioSource.clip);
                    bulletSystem.Shoot();
                }
            }
        }
    }
    void moveBoss()
    {
        if (phase == 1)
        {
            float timeValue = Time.time - startTime,
                radius = 35,
                xVelocity = radius * (-petals * Mathf.Sin(petals * speed * timeValue) * Mathf.Sin(speed * timeValue) + Mathf.Cos(petals * speed * timeValue) * Mathf.Cos(speed * timeValue)) + 
                                horizontalBound / 2 * speed * Mathf.Cos(speed * timeValue),
                zVelocity = radius * (-petals * Mathf.Sin(petals * speed * timeValue) * Mathf.Cos(speed * timeValue) - Mathf.Cos(petals * speed * timeValue) * Mathf.Sin(speed * timeValue)) -
                                (upperVerticalBound + lowerVerticalBound) / 8 * speed *  Mathf.Sin(speed * timeValue);

            rb.velocity = new Vector3(xVelocity, 0, zVelocity);
        }
        else if(phase == 2)
        {
            float timeValue = Time.time - startTime,
                xVelocity = horizontalBound * speed * Mathf.Cos(speed * timeValue),
                zVelocity = -60;

            rb.velocity = new Vector3(xVelocity, 0, zVelocity);
        }
        else if (phase == 3)
        {

            Vector3 goToPos = new Vector3(0, 0, 40);
            if (Vector3.Distance(tnf.position, goToPos) < 1)
                moveInPattern = true;

            if (!moveInPattern)
            {
                rb.velocity = Vector3.zero;
                tnf.position = Vector3.MoveTowards(tnf.position, goToPos, 1f);
            }
            else
            {
                float timeValue = Time.time - startTime,
                xVelocity = horizontalBound * speed * Mathf.Cos(speed * timeValue),
                zVelocity = -(upperVerticalBound + lowerVerticalBound) / 3 * speed * Mathf.Sin(speed * timeValue);

                rb.velocity = new Vector3(xVelocity, 0, zVelocity);
            }
        }
    }

    void boundMovement()
    {
        if(phase == 1)
        {
            Vector3 nextPostition = transform.position;
            nextPostition.x = Mathf.Clamp(nextPostition.x + rb.velocity.x, -horizontalBound, horizontalBound);
            nextPostition.z = Mathf.Clamp(nextPostition.z + rb.velocity.x, lowerVerticalBound, upperVerticalBound);

            transform.position = Vector3.Lerp(transform.position, nextPostition, Time.deltaTime);
        }
        else if(phase == 2)
        {
            if (tnf.position.z < -90)
                tnf.position = new Vector3(tnf.position.x, 0, Mathf.Abs(tnf.position.z));

            if (tnf.position.z < -130)
                tnf.position = new Vector3(Mathf.Abs(tnf.position.x), 0, tnf.position.z);
            else if (tnf.position.z > 130)
                tnf.position = new Vector3(tnf.position.x * -1, 0, tnf.position.z);
        }
    }

    void phaseCheck()
    {
        if (health <= 0)
        {
            GameObject.Find("PersistentObject").GetComponent<PersistentController>().DecEnemyCount(GetComponent<BossHandler>().GetPoints());
            Destroy(gameObject);
        }
        else if (health <= maxHealth / 3)
        {
            phase = 3;
        }
        else if (health <= (2 * maxHealth / 3))
        {
            phase = 2;
        }
        else
        {
            phase = 1;
        }
    }

    public void TakeDamage()
    {
        health -= 75;
    }

    public int GetPoints()
    {
        return points;
    }

    public void SetSpeed(float spd)
    {
        this.speed = spd;
    }

    public void SetAggroLevel(float aggroLevel)
    {
        maxHealth = 1000 * (int)aggroLevel;
        health = maxHealth;
        burstRate = 1.0f * aggroLevel;
        burstCount = 5 * (int)aggroLevel;
        fireSpeed = 0.36f / aggroLevel;
}

    public void SetPoints(int points)
    {
        this.points = points;
    }
}
