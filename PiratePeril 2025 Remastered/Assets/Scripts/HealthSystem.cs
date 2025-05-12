using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth;

    private int curHealth;
    public int CurHealth { get => curHealth; }

    private void Awake()
    {
        curHealth = maxHealth;
    }

    public void TakeDamage(int damage, DamageOptions damageOptions)
    {
        curHealth -= damage;
        this.gameObject.BroadcastMessage("DamageTaken", damageOptions, SendMessageOptions.DontRequireReceiver);
        if(curHealth <= 0)
        {
            this.gameObject.BroadcastMessage("Death", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void AddHealth(int health)
    {
        curHealth += health;
        if (curHealth > maxHealth)
            curHealth = maxHealth;
    }
}
