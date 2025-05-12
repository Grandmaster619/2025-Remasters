using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Values")]
    public float moveVel = 20f;
    public float jumpForce = 200f;
    public float jumpCD = 0.1f;
    public int maxJumps;

    [Header("Combat Values")]
    public float attackRadius;
    public float attackCD = 0.5f;
    public int attackDamage = 1;
    public float attackForce;

    [Header("Resources")]
    public AudioSource jumpSound;
    public AudioSource attackSound;
    public GameObject deathMenu;

    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 moveVec;
    private Vector3 velocity = Vector3.zero;
    private bool onGround = false, jump = false, facingRight = true, dead = false;
    private float timeSinceAttack, timeSinceJump;
    private int numJumps;

    private PersistentControl persistentObject;

    private void Awake()
    {
        persistentObject = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentControl>();


        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        timeSinceAttack = attackCD;
        timeSinceJump = jumpCD;
        numJumps = maxJumps;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!dead)
        {
            timeSinceAttack += Time.deltaTime;
            timeSinceJump += Time.deltaTime;
            moveVec = new Vector2(0, 0);

            anim.SetFloat("velocityVert", rb.velocity.y);
            CheckAnimation();
            CheckMovement();

            if (Input.GetKey(KeyCode.Mouse0) && timeSinceAttack >= attackCD)
            {
                PerformAttack();
                attackSound.PlayOneShot(attackSound.clip);
                timeSinceAttack = 0;
            }
        }
    }

    private void CheckAnimation()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            anim.SetBool("walking", true);
        }
        else if(Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("walking", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetBool("walking", true);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("walking", false);
        }
        if(timeSinceAttack >= 0.15)
        {
            anim.SetBool("attacking", false);
        }
    }

    private void CheckMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            moveVec.x = -moveVel;
            transform.localRotation = new Quaternion(0, 180, 0, 0);
            facingRight = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveVec.x = moveVel;
            transform.localRotation = new Quaternion(0, 0, 0, 0);
            facingRight = true;
        }

        if (Input.GetKeyDown(KeyCode.W) && (onGround || (numJumps > 0 && numJumps < maxJumps && maxJumps > 1)) && timeSinceJump >= jumpCD && !jump)
        {
            anim.SetBool("jumping", true);
            jumpSound.PlayOneShot(jumpSound.clip);
            timeSinceJump = 0;
            jump = true;
            numJumps--;
        }
    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, moveVec, ref velocity, 0.25f);
            if (jump)
            {
                rb.AddForce(new Vector2(0, jumpForce));
                jump = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Structure"))
        {
            onGround = true;
            numJumps = maxJumps;
            anim.SetBool("jumping", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Structure"))
            onGround = false;
    }

    public void DamageTaken(DamageOptions damageOptions)
    {
        Vector2 positionFrom = damageOptions.Source.transform.position;
        if (positionFrom.x < transform.position.x)
            rb.AddForce(new Vector2(damageOptions.Force, damageOptions.Force));
        else
            rb.AddForce(new Vector2(-damageOptions.Force, damageOptions.Force));
        attackSound.PlayOneShot(attackSound.clip);
    }

    public void PerformAttack()
    {
        anim.SetBool("attacking", true);
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D collider in results)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                if((facingRight && collider.transform.position.x > transform.position.x) || 
                    (!facingRight && collider.transform.position.x < transform.position.x))
                {
                    DamageOptions damage = new DamageOptions(this.gameObject, attackForce);
                    collider.transform.GetComponent<HealthSystem>().TakeDamage(attackDamage, damage);
                }
            }
        }
    }

    public void Death()
    {
        deathMenu.transform.GetChild(0).gameObject.SetActive(true);
        dead = true;
        persistentObject.Collected.Clear();
    }

    public void UpdateCollectibles()
    {
        if (persistentObject.Collected.Contains(Collectibles.JUMPING_JEWEL))
            maxJumps = 2;
        if (persistentObject.Collected.Contains(Collectibles.CANDESCENT_CUTLASS))
            attackDamage = 2;
    }
}
