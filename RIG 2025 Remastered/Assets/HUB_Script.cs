using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUB_Script : MonoBehaviour
{
    private RectTransform healthBar;
    private RectTransform staminaBar;
    private Health health;
    private Stamina stamina;

    public float leftBound = 0f;
    public float rightBound = 265f;

    private float boundDifference;
    private float stepAmountHealth;
    private float stepAmountStamina;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = gameObject.transform.Find("HealthBar").GetComponent<RectTransform>();
        staminaBar = gameObject.transform.Find("StaminaBar").GetComponent<RectTransform>();
        health = GameObject.Find("FirstPersonPlayer").GetComponent<Health>();
        stamina = GameObject.Find("FirstPersonPlayer").GetComponent<Stamina>();
        boundDifference = leftBound - rightBound;
        stepAmountHealth = boundDifference / health.maxHealth;
        stepAmountStamina = boundDifference / stamina.MAX_STAMINA;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.sizeDelta = new Vector2(leftBound - health.currentHealth * stepAmountHealth, healthBar.sizeDelta.y);
        staminaBar.sizeDelta = new Vector2(leftBound - stamina.stamina_level * stepAmountStamina, staminaBar.sizeDelta.y);
    }
}
