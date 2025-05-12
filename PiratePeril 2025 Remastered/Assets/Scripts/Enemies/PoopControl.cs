using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopControl : MonoBehaviour
{
    public float damageForce;
    public int damageValue;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            DamageOptions damage = new DamageOptions(this.gameObject, damageForce);
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damageValue, damage);
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.CompareTag("Structure"))
        {
            Destroy(this.gameObject);
        }
    }
}
