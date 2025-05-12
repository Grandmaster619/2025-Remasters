using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level_1_Goals : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private PassCheck passCheckScript;
    [SerializeField] private string firstGoal;
    [SerializeField] private string secondGoal;

    // Start is called before the first frame update
    void Start()
    {
        passCheckScript.OnKeycodeCorrect += Goals_OnKeyCodeCorrect;
        goalText.text = firstGoal;
    }

    private void Goals_OnKeyCodeCorrect(object sender, EventArgs e)
    {
        goalText.text = secondGoal;
    }
}
