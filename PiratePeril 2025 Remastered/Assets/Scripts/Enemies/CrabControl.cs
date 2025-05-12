using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabControl : MonoBehaviour
{
    public float moveSpeed;
    public GameObject player;
    public float looseness = 0.25f;
    public float damageForce = 250f;
    public float damageCD = 0.5f;
    public float detectionRadius;
    public int damageValue;

    private bool playerInVision;
    private Vector2 moveVec;
    private Vector3 velocity = Vector3.zero;
    private float timeSinceAttack = 0;

    private Animator anim;
    private Rigidbody2D rb;
    


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        moveVec = new Vector2(0, 0);
        bool walking = false;
        if(CheckIfPlayerInRadius())
        {
            if(player.transform.position.x < transform.position.x)
            {
                moveVec.x = -moveSpeed;
                walking = true;
            }
            else if(player.transform.position.x > transform.position.x)
            {
                moveVec.x = moveSpeed;
                walking = true;
            }
        }
        anim.SetBool("walking", walking);
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.SmoothDamp(rb.velocity, moveVec, ref velocity, 0.25f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && timeSinceAttack >= damageCD)
        {
            DamageOptions damage = new DamageOptions(this.gameObject, damageForce);
            player.GetComponent<HealthSystem>().TakeDamage(damageValue, damage);
            timeSinceAttack = 0;
        }
    }

    private bool CheckIfPlayerInRadius()
    {
        bool playerFound = false;
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach(Collider2D collider in results)
        {
            if (collider.gameObject.CompareTag("Player"))
                playerFound = true;
        }
        return playerFound;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerInVision = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerInVision = false;
    }

    public void DamageTaken(DamageOptions damageOptions)
    {
        Vector2 positionFrom = damageOptions.Source.transform.position;
        if (positionFrom.x < transform.position.x)
            rb.AddForce(new Vector2(damageOptions.Force, damageOptions.Force));
        else
            rb.AddForce(new Vector2(-damageOptions.Force, damageOptions.Force));
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
}
