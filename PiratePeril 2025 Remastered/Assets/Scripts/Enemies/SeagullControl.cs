using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullControl : MonoBehaviour
{
    public float moveSpeed;
    public float poopMaxCD;
    public float poopMinCD;
    public GameObject poopPrefab;

    private Vector2 moveVec;
    private Rigidbody2D rb;
    private float timeSinceTurn, timeSincePoop, poopCD;

    private Vector3 velocity = Vector3.zero;
    private Vector3 origin;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        origin = transform.position;
        moveVec.x = moveSpeed;
        timeSinceTurn = 0;
        poopCD = Random.Range(poopMinCD, poopMaxCD);
    }

    private void Update()
    {
        timeSinceTurn += Time.deltaTime;
        timeSincePoop += Time.deltaTime;
        if (Mathf.Abs(transform.position.x - origin.x) >= 3 && timeSinceTurn >= 2)
        {
            moveVec.x = -moveVec.x;
            timeSinceTurn = 0;
        }
        if(rb.velocity.x > 0)
            transform.localRotation = new Quaternion(0, 180, 0, 0);
        else
            transform.localRotation = new Quaternion(0, 0, 0, 0);

        if(timeSincePoop >= poopCD)
        {
            Vector2 spawnPos = transform.position;
            spawnPos.y -= 0.25f;
            GameObject.Instantiate(poopPrefab, spawnPos, new Quaternion());
            timeSincePoop = 0;
            poopCD = Random.Range(poopMinCD, poopMaxCD);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.SmoothDamp(rb.velocity, moveVec, ref velocity, 0.25f);
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveVec.x = -moveVec.x;
        timeSinceTurn = 0;
    }
}
