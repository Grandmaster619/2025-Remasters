using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI brownTotal;
    [SerializeField] private TextMeshProUGUI greyTotal;
    [SerializeField] private TextMeshProUGUI tealTotal;
    [SerializeField] private TextMeshProUGUI redTotal;
    [SerializeField] private TextMeshProUGUI yellowTotal;
    [SerializeField] private TextMeshProUGUI greenTotal;
    [SerializeField] private TextMeshProUGUI blackTotal;

    private PersistentController persistent;

    // Start is called before the first frame update
    void Awake()
    {
        persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentController>();
        int totalScore = persistent.TanksKilled[TankType.BROWN] + persistent.TanksKilled[TankType.GREY] +
            persistent.TanksKilled[TankType.TEAL] + persistent.TanksKilled[TankType.RED] + persistent.TanksKilled[TankType.YELLOW] +
            persistent.TanksKilled[TankType.GREEN] + persistent.TanksKilled[TankType.BLACK];
        totalScoreText.text = totalScore.ToString();
        brownTotal.text = persistent.TanksKilled[TankType.BROWN].ToString();
        greyTotal.text = persistent.TanksKilled[TankType.GREY].ToString();
        tealTotal.text = persistent.TanksKilled[TankType.TEAL].ToString();
        redTotal.text = persistent.TanksKilled[TankType.RED].ToString();
        yellowTotal.text = persistent.TanksKilled[TankType.YELLOW].ToString();
        greenTotal.text = persistent.TanksKilled[TankType.GREEN].ToString();
        blackTotal.text = persistent.TanksKilled[TankType.BLACK].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
