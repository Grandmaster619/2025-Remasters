using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveController : MonoBehaviour
{
    [SerializeField] private float enemyMultiplier, roundMultiplier, roundMultiplierTracker, baseMultiplier, exponentialAdjuster, horizontalBound, verticalBound;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject[] prefabs;

    private int[] spawnPattern;
    private int currentRound, bossFrequency, enemiesPerLevel;
    private Canvas WaveCanvas;
    private Text waveText;
    private PersistentController PersistentInfo;
    private List<GameObject> enemies = new List<GameObject>();
    private const int ROWS = 4;
    private const int COLUMNS = 10;
    private float enemyMovementDelay = 3f, nextEnemyMovementTime;
    private const float WAVE_GRACE_TIME = 8f;
    private System.Random rand;
    private float startTime;
    private int A, B;

    private void Start()
    {
        rand = new System.Random();
        currentRound = 0;
        bossFrequency = 10;
        enemiesPerLevel = 40;
        nextEnemyMovementTime = WAVE_GRACE_TIME;
        enemyMultiplier = roundMultiplierTracker = 1f;
        roundMultiplier = 1.3f;
        exponentialAdjuster = .052f;
        baseMultiplier = (.75f - exponentialAdjuster);

        PersistentInfo = GameObject.Find("PersistentObject").GetComponent<PersistentController>();

        WaveCanvas = GameObject.Find("WaveCanvas").GetComponent<Canvas>();
        waveText = WaveCanvas.GetComponentInChildren<Text>();
        WaveCanvas.enabled = false;
        waveText.text = "Wave: " + currentRound;
    }

    void FixedUpdate()
    {

        if (PersistentInfo.GetEnemyCount() == 0 && !WaveCanvas.enabled)
        {
            IncrementRound();
            
            if(currentRound % 10 == 0)
            {
                PersistentInfo.SetEnemyCount(1);
                StartCoroutine("GenerateBossWave");
            }
            else 
            {
                PersistentInfo.SetEnemyCount(enemiesPerLevel);
                StartCoroutine("GenerateWave");
            }

            transform.position = Vector3.zero;
            startTime = 0;
            A = Random.Range(1, 3);
            B = Random.Range(0, 2);
        }
        if (Time.time > nextEnemyMovementTime)
        {
            if (enemies.Count > 0)
            {
                List<GameObject> frontEnemies = FindFrontEnemies();
                if (frontEnemies.Count > 0)
                {
                    GameObject selectedEnemy = frontEnemies[rand.Next(0, frontEnemies.Count)];
                    int xPathIndex = rand.Next(0, 2);
                    int yPathIndex = rand.Next(2, 5);

                    selectedEnemy.GetComponent<Enemy_Movement>().StartMovement(xPathIndex, yPathIndex, 0);
                }
            }
            nextEnemyMovementTime += enemyMovementDelay;
        }

        if (currentRound % 10 != 0 && PersistentInfo.GetEnemyCount() > 0 && !WaveCanvas.enabled)
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(horizontalBound * Mathf.Sin(A * (Time.time - startTime)), 0,
                verticalBound * Mathf.Cos(B * (Time.time - startTime)) - 7), 0.1f);
        }
    }

    private List<GameObject> FindFrontEnemies()
    {
        List<GameObject> frontEnemies = new List<GameObject>();
        for (int curCol = 0; curCol < COLUMNS; curCol++)
        {
            bool shipFound = false;
            for (int curRow = ROWS - 1; curRow >= 0 && !shipFound; curRow--)
            {
                int index = curCol + ((curRow) * COLUMNS);
                if (enemies[index] != null && !enemies[index].GetComponent<Enemy_Movement>().isMoving)
                {
                    shipFound = true;
                    frontEnemies.Add(enemies[index]);
                }
            }
        }
        return frontEnemies;
    }

    /*
     * Calculates the enemy adjuster for the next round
     */
    public void IncrementRound()
    {
        enemies.Clear();
        currentRound += 1;
        nextEnemyMovementTime = WAVE_GRACE_TIME + Time.time;
        if (currentRound % bossFrequency == 0)
        {
            roundMultiplierTracker = 1; //reset exponential aspect of multiplier
            baseMultiplier += .25f; //increase base multiplier for next set of rounds
        }
        else
            roundMultiplierTracker *= roundMultiplier;
        enemyMultiplier = Mathf.Clamp((exponentialAdjuster * roundMultiplierTracker) + baseMultiplier, 1, 4);

    }

    public float GetEnemyMultiplier()
    {
        return this.enemyMultiplier;
    }

    private int[] GenerateSpawnPattern()
    {
        spawnPattern = new int[enemiesPerLevel];
        for (int j = 0; j < (currentRound - 1) * 3; j++)
        {
            int k = Random.Range(0, (enemiesPerLevel / 2));

            if (k < 5)
            {
                spawnPattern[k] = 1;
                spawnPattern[9 - k] = 1;
            }
            else if (k < 10)
            {
                spawnPattern[5 + k] = 1;
                spawnPattern[24 - k] = 1;
            }
            else if (k < 15)
            {
                spawnPattern[10 + k] = 1;
                spawnPattern[39 - k] = 1;
            }
            else
            {
                spawnPattern[15 + k] = 1;
                spawnPattern[54 - k] = 1;
            }
        }

        bool[] usedValue = new bool[enemiesPerLevel / 4];
        for (int j = 0; j < Mathf.Clamp((currentRound - 4), 0, 5); j++)
        {
            int k = Random.Range(0, (enemiesPerLevel / 4));

            if (usedValue[k])
                j--;
            else
                usedValue[k] = true;

            if (k < 5)
            {
                spawnPattern[k] = 2;
                spawnPattern[9 - k] = 2;
            }
            else
            {
                spawnPattern[5 + k] = 2;
                spawnPattern[24 - k] = 2;
            }
        }


        return spawnPattern;
    }

    private IEnumerator GenerateWave()
    {
        WaveCanvas.enabled = true;
        waveText.text = "Wave: " + currentRound;

        yield return new WaitForSeconds(1.5f);

        GenerateSpawnPattern();

        for (int i = 0; i < enemiesPerLevel; i++)
        {

            int prefabIndex = spawnPattern[i];
            GameObject enemy = Instantiate(prefabs[prefabIndex],
                new Vector3(-45 + 10 * (i % 10), 0, 70 - 10 * (int)(i / 10)),
                Quaternion.identity,
                transform);

            float speed, aggro;
            int points;
            switch (prefabIndex)
            {

                default:
                    speed = 120 * enemyMultiplier;
                    aggro = 1 * enemyMultiplier;
                    points = 100 + (50 * ((currentRound - 1) / bossFrequency));
                    break;
                case 1:
                    speed = 140 * enemyMultiplier;
                    aggro = 1.2f * enemyMultiplier;
                    points = 125 + (75 * ((currentRound - 1) / bossFrequency));
                    break;
                case 2:
                    speed = 160 * enemyMultiplier;
                    aggro = 1.4f * enemyMultiplier;
                    points = 150 + (100 * ((currentRound - 1) / bossFrequency));
                    break;
            }
            
            enemy.GetComponent<Enemy_Movement>().index = i+1;
            enemy.GetComponent<EnemyController>().SetSpeed(speed);
            enemy.GetComponent<EnemyController>().SetAggroLevel(aggro);
            enemy.GetComponent<EnemyController>().SetPoints(points);
            enemies.Add(enemy);
        }

        WaveCanvas.enabled = false;
    }

    private IEnumerator GenerateBossWave()
    {
        WaveCanvas.enabled = true;
        waveText.text = "Wave: " + currentRound;

        yield return new WaitForSeconds(1.5f);

        GameObject enemy = Instantiate(bossPrefab, 
                new Vector3(0, 0, 50),
                Quaternion.identity,
                transform);

        float speed = 1 * enemyMultiplier;
        float aggro = 1.8f * enemyMultiplier;
        int points = 1000 * (currentRound / 10);

        // Set Boss Handler Values
        enemy.GetComponent<BossHandler>().SetSpeed(speed);
        enemy.GetComponent<BossHandler>().SetAggroLevel(aggro);
        enemy.GetComponent<BossHandler>().SetPoints(points);

        WaveCanvas.enabled = false;
    }
}
