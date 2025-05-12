using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoController : MonoBehaviour
{
    private PersistentController PersistentInfo;
    private Text LiveText, ScoreText, LiveUpText;
    // Start is called before the first frame update
    void Awake()
    {
        PersistentInfo = GameObject.Find("PersistentObject").GetComponent<PersistentController>();
        LiveText = GameObject.Find("LiveText").GetComponent<Text>();
        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        LiveUpText = GameObject.Find("LiveUpText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        LiveText.text = "Lives: " + PersistentInfo.GetLives();
        ScoreText.text = "Score: " + PersistentInfo.GetPlayerScore().ToString();
        LiveUpText.text = "Next Life: " + PersistentInfo.GetAdditionalLife().ToString();
    }
}
