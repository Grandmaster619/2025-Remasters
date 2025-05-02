using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject playerPrefab;

    private PersistentController persistentController;
    private Vector3 playerSpawn;
    private bool restarting = false, gggonext = false;

    private void Awake()
    {
        persistentController = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentController>();
        playerSpawn = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void Update()
    {
        // scan level for enemies, if there are none then the level is complete successfully.
        // also scan for player, if there is no player then the level is unsuccessful and must be restarted.

        if(GameObject.FindGameObjectsWithTag("Player").Length == 0 && !restarting)
        {
            // git gud try again
            // if the player has enough lives, they can try the level over, and any tanks they killed in previous attempts will not respawn
            if(persistentController.CurLives > 1)
                StartCoroutine(RestartLevel());
            else
            {
                FindObjectOfType<AudioManager>().Stop("theme");
                persistentController.ToEndScreen();
            }
        }
        else if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !gggonext)
        {
            // gg go next
            
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator RestartLevel()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        
        restarting = true;
        persistentController.CurLives -= 1;
        yield return new WaitForSeconds(1.5f);
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyMovement>().ReturnToStart();
        }
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        for (int i = 0; i < bullets.Length; i++)
            Destroy(bullets[i]);

        GameObject.Instantiate(playerPrefab, playerSpawn, new Quaternion());

        Time.timeScale = 0;
        float pauseEndTime = Time.realtimeSinceStartup + 1.5f;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1f;
        restarting = false;
    }

    IEnumerator NextLevel()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        gggonext = true;
        FindObjectOfType<AudioManager>().Play("victory");
        yield return new WaitForSeconds(2);

        persistentController.CurLevel += 1;
        persistentController.LoadPlayScene(persistentController.CurLevel);
    }
}
