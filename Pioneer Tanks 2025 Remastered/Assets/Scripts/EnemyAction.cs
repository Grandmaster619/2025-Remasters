using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    // Fields
    [SerializeField] public Transform emissionPoint; // The position to shoot the bullet from
    [SerializeField] public GameObject bullet;
    [SerializeField] public GameObject turret;
    [SerializeField] private float attackTimerCD = 0;
    [SerializeField] private int maxBulletStorage = 5;
    [SerializeField] private float reloadTimerCD = 2;
    [SerializeField] private float bulletSpeed = 6;
    [SerializeField] private GameObject effect;
    [SerializeField] private Transform particleEmissionPoint;
    [SerializeField] private float turretRotationSpeed;
    [SerializeField] private int ricochetCount = 1;
    [SerializeField] private float movePauseCD = 0.5f;
    [SerializeField] private bool isTeal = false;
    [SerializeField] private bool isGreen = false;


    private float x, z;
    private float y = 0;
    private float switchDirFreq = 0;
    private float attackTimer = 0, reloadTimer = 0, switchDirTimer = 0, movePauseTimer = 0, mineTimer = 0, targetTimer = 0;
    private int curBullets, curMines;
    private PersistentController persistent;
    private AudioManager audio;
    private GameObject player;

    Rigidbody rigidBody;

    private void Awake()
    {
        curBullets = maxBulletStorage;
        switchDirFreq = Random.Range(1f, 2f);
        persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentController>();
        audio = GameObject.FindObjectOfType<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private int VectorSum(RaycastHit vec1, RaycastHit vec2, RaycastHit vec3, RaycastHit vec4)
    {
        int sum = 0;
        if (vec1.transform.CompareTag("Player"))
            sum += 1;
        if (vec2.transform.CompareTag("Player"))
            sum += 1;
        if (vec3.transform.CompareTag("Player"))
            sum += 1;
        if (vec4.transform.CompareTag("Player"))
            sum += 1;
        return sum;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        attackTimer += Time.deltaTime;
        reloadTimer += Time.deltaTime;
        movePauseTimer += Time.deltaTime;
        mineTimer += Time.deltaTime;
        targetTimer += Time.deltaTime;

        if (!persistent.PAUSED)
        {
            if (reloadTimer >= reloadTimerCD)
            {
                curBullets = maxBulletStorage;
                reloadTimer = 0;
            }

            if (movePauseTimer >= movePauseCD)
            {
                GetComponent<EnemyMovement>().PauseToShoot = false;
            }
            float thingy = 0.075f;
            Vector3 aimVector = new Vector3(Mathf.Cos(turretAngleTrig(turret.transform.eulerAngles.y) * Mathf.Deg2Rad), 0f, Mathf.Sin(turretAngleTrig(turret.transform.eulerAngles.y) * Mathf.Deg2Rad));
            Vector3 pos1 = new Vector3(turret.transform.position.x + thingy, transform.position.y + 0.25f, turret.transform.position.z);
            Vector3 pos2 = new Vector3(turret.transform.position.x - thingy, transform.position.y + 0.25f, turret.transform.position.z);
            Vector3 pos3 = new Vector3(turret.transform.position.x, transform.position.y + 0.25f, turret.transform.position.z + thingy);
            Vector3 pos4 = new Vector3(turret.transform.position.x , transform.position.y + 0.25f, turret.transform.position.z - thingy);
            RaycastHit objFound = AimBounce(turret.transform.position, aimVector, ricochetCount);
            RaycastHit objFound1 = AimBounce(pos1, aimVector, ricochetCount);
            RaycastHit objFound2 = AimBounce(pos2, aimVector, ricochetCount);
            RaycastHit objFound3 = AimBounce(pos3, aimVector, ricochetCount);
            RaycastHit objFound4 = AimBounce(pos4, aimVector, ricochetCount);
            try
            {
                if (VectorSum(objFound1, objFound2, objFound3, objFound4) == 4 && attackTimer >= attackTimerCD && curBullets > 0)
                {
                    if (isTeal)
                        audio.Play("tealShot");
                    else if (isGreen)
                        audio.Play("greenShot");

                    // fire bullet
                    targetTimer = 0;
                    GetComponent<EnemyMovement>().PauseToShoot = true;
                    movePauseTimer = 0;

                    attackTimer = 0;
                    reloadTimer = 0;
                    curBullets -= 1;
                    GameObject newBullet = Instantiate(bullet, emissionPoint.transform.position, new Quaternion());
                    newBullet.GetComponent<Bullet>().maxBounces = ricochetCount;
                    newBullet.GetComponent<Bullet>().bulletSpeed = bulletSpeed;

                    rigidBody = newBullet.GetComponent<Rigidbody>();
                    //rigidBody.centerOfMass = new Vector3(0, 0, -1);
                    x = bulletSpeed * (Mathf.Cos(turretAngleTrig(turret.transform.eulerAngles.y) * Mathf.Deg2Rad));
                    z = bulletSpeed * (Mathf.Sin(turretAngleTrig(turret.transform.eulerAngles.y) * Mathf.Deg2Rad));

                    rigidBody.velocity = new Vector3(x, 0, z);
                    StartCoroutine(EmitParticles());

                    //newBullet.GetComponent<Bullet>().fire(emissionPoint.position, turret.transform.localEulerAngles, 3.5f);
                }
                else
                {
                    float turretSpeed = turretRotationSpeed;
                    RaycastHit playerVisible;
                    Vector3 playerDirection = player.transform.position - transform.position;
                    Vector3 rayStart = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
                    if (Physics.Raycast(rayStart, playerDirection, out playerVisible))
                    {
                        if (playerVisible.transform.CompareTag("Player"))
                        {
                            
                            float xDiff = player.transform.position.x - transform.position.x;
                            float zDiff = player.transform.position.z - transform.position.z;
                            //Debug.Log(xDiff + ", " + zDiff);
                            float desiredAngle = Mathf.Atan(zDiff / xDiff) * Mathf.Rad2Deg;

                            //Debug.Log(desiredAngle);
                            if (xDiff < 0 && zDiff >= 0)
                                desiredAngle += 180;
                            else if (xDiff < 0 && zDiff < 0)
                                desiredAngle += 180;
                            else if (xDiff >= 0 && zDiff < 0)
                                desiredAngle += 360;
                            Debug.Log("Desired: " + desiredAngle + ", Actual: " + turretAngleTrig2(turret.transform.eulerAngles.y));
                            //Debug.Log(turret.transform.eulerAngles.y % 360);
                            float angleLeniency = 0;
                            if (Mathf.Abs(turretAngleTrig2(turret.transform.eulerAngles.y) - (desiredAngle)) > angleLeniency)
                            {
                                if (turretAngleTrig2(turret.transform.eulerAngles.y) < 90f && desiredAngle > 270f)
                                    turretRotationSpeed = Mathf.Abs(turretRotationSpeed);
                                else if (turretAngleTrig2(turret.transform.eulerAngles.y) > 270f && desiredAngle < 90f)
                                    turretRotationSpeed = Mathf.Abs(turretRotationSpeed) * -1;
                                else if (turretAngleTrig2(turret.transform.eulerAngles.y) < (desiredAngle))
                                {
                                    turretRotationSpeed = Mathf.Abs(turretRotationSpeed) * -1;
                                }
                                else
                                {
                                    turretRotationSpeed = Mathf.Abs(turretRotationSpeed);
                                }
                            }
                            turretSpeed = turretRotationSpeed * 3;
                        }
                        else
                        {
                            switchDirTimer += Time.deltaTime;
                            if (switchDirTimer >= switchDirFreq)
                            {
                                switchDirFreq = Random.Range(1f, 2f);
                                switchDirTimer = 0;
                                turretRotationSpeed *= -1;
                            }
                        }

                    }
                    if (movePauseTimer >= movePauseCD && targetTimer >= 0)
                        turret.transform.Rotate(new Vector3(0, 0, turretSpeed));
                }
            }
            catch
            {
                Debug.Log("error caught");
            }
        }

        

    }

    float turretAngleTrig(float angle)
    {
        if (angle > 0)
            return 90 + (90 - angle);
        else
            return -90 - (-90 + angle);
    }

    float turretAngleTrig2(float angle)
    {
        if (angle < 180)
            return 90 + (90 - angle);
        else
            return 270 + (270 - angle);
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


    private RaycastHit AimBounce(Vector3 startPos, Vector3 normalizedDir, int numBounces, RaycastHit lastHit = new RaycastHit())
    {
        //Debug.Log("Bounce number: " + numBounces + "Reflection Vector: " + normalizedDir);
        if (numBounces >= 1)
        {
            RaycastHit hit;
            // Does the ray intersect any Walls, Players, or Enemies
            if (Physics.Raycast(startPos, normalizedDir, out hit))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    Debug.DrawLine(startPos, hit.point, Color.green);
                    return hit;
                }
                else if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    Debug.DrawLine(startPos, hit.point, Color.red);
                    return hit;
                }
                else
                {
                    Debug.DrawLine(startPos, hit.point, Color.white);
                    // Find the line from the gun/wall to the point that was clicked.
                    Vector3 incomingVec = hit.point - startPos;

                    // Use the point's normal to calculate the reflection vector.
                    Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
                    //Debug.Log("Bounce number: " + numBounces + "Reflection Vector: " + reflectVec);
                    return AimBounce(hit.point + (reflectVec.normalized * 0.1f), reflectVec.normalized, numBounces - 1, hit);
                }
            }

        }
        else
        {
            RaycastHit hit;
            // Does the ray intersect any Walls, Players, or Enemies
            if (Physics.Raycast(startPos, normalizedDir, out hit))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                    Debug.DrawLine(startPos, hit.point, Color.green);
                else if (hit.transform.gameObject.CompareTag("Enemy"))
                    Debug.DrawLine(startPos, hit.point, Color.red);
                else
                    Debug.DrawLine(startPos, hit.point, Color.black);
                return hit;
            }
        }
        return lastHit;
    }
}
