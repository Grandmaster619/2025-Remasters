using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDifficultyV2 : MonoBehaviour
{
    [SerializeField]
    private float enemyMultiplier, roundMultiplier, roundMultiplierTracker, baseMultiplier, exponentialAdjuster;
    private int currentRound, bossFrequency;

    private float timer;

    private void Start()
    {
        currentRound = 0;
        bossFrequency = 10;

        enemyMultiplier = roundMultiplierTracker = 1f;
        roundMultiplier = 1.3f;
        exponentialAdjuster = .052f;
        baseMultiplier = (1 - exponentialAdjuster);

        timer = 0f;
    }

    private void Update()
    {
        /*
        timer += Time.deltaTime;
        if(timer > 0.5f)
        {
            Debug.Log(enemyMultiplier);
            IncrementRound();
            timer = 0;
        }
        */
    }

    /*
     * Calculates the enemy adjuster for the next round
     */
    public void IncrementRound()
    {
        currentRound++;
        if (currentRound % bossFrequency == 0)
        {
            roundMultiplierTracker = 1; //reset exponential aspect of multiplier
            baseMultiplier += .25f; //increase base multiplier for next set of rounds
        }
        else
            roundMultiplierTracker *= roundMultiplier;
        enemyMultiplier = (exponentialAdjuster * roundMultiplierTracker) + baseMultiplier;

    }

    public float GetEnemyMultiplier()
    {
        return this.enemyMultiplier;
    }
}
