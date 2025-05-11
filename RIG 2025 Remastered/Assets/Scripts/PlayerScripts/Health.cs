using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Health : MonoBehaviour
{
    private static Health instance;

    public EventHandler<EventArgs> OnPlayerTakeDamage;
    public EventHandler<EventArgs> OnPlayerDies;

    private Stamina stamina;

    public int maxHealth = 100;
    public int currentHealth;
    [Space]
    public float regenDowntime = 3;
    public float regenerationFrequency = 0.8f;
    [Space]
    public float immunityCooldown = 1.5f;

    //private float regeneration_downtime_timer = 0;
    private float immunity_cooldown_timer = 0;
    //private float regeneration_frequency_timer = 0;

    public GameObject DeathScreen;

    [HideInInspector]
    public bool isAlive = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.Log("An instance of PauseScript already exists");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        stamina = gameObject.GetComponent<Stamina>();
    }

    void Update()
    {
        
            // Immunity frame cooldown after taking damage
        if (immunity_cooldown_timer > 0)
        {
            immunity_cooldown_timer -= Time.deltaTime;
        }

        // Passive Regeneration
        /*if (regeneration_downtime_timer > 0)
        {
            regeneration_downtime_timer -= Time.deltaTime;
        }
        else if (regeneration_frequency_timer > 0)
        {
            regeneration_frequency_timer -= Time.deltaTime;
        }
        else
        {
            Heal(1);
            regeneration_frequency_timer = regenerationFrequency;
        }*/

        // Detection if player should die
        if (currentHealth <= 0 && isAlive)
        {
            PlayerDied();
        }

    }

    public void TakeDamage(int amount)
    {
        // Play damage sound
        if (immunity_cooldown_timer <= 0)
        {
            // *** Player Take Damage Event ***
            OnPlayerTakeDamage?.Invoke(this, new EventArgs());
            currentHealth -= amount;
            //regeneration_downtime_timer = regenDowntime;
            immunity_cooldown_timer = immunityCooldown;
        }
    }


    public void Heal(int amount)
    {

        // TODO: Play heal sound
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void PlayerDied()
    {
        OnPlayerDies?.Invoke(this, new EventArgs());
        isAlive = false;
        Debug.Log("Player had Died!");
        DeathScreen.SetActive(true);
        Time.timeScale = 0f;
        //todo: Death menu animations (lower priority)
        //todo: Better death menu visuals
        //todo: Better death menu music

    }

    public static Health GetInstance() { return instance; }
}
