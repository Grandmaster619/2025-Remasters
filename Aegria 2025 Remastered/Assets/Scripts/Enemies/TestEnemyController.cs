using UnityEngine;

public class TestEnemyController : MonoBehaviour
{
    private Transform player;
    private ParticleLauncher pl;

    private void Start()
    {
        // Test enemy driver
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pl = GetComponent<ParticleLauncher>();
        InvokeRepeating("Shoot", 1f, 2f);
    }

    private void Update()
    {
        if (player != null)
            transform.rotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
    }

    private void Shoot()
    {
        pl.Shoot();
    }
}