using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    [SerializeField] private GameObject explosionSphere;
    [SerializeField] private GameObject defaultGFX;

    public void Explode()
    {
        StartCoroutine(DoExplosion());
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Destructible"))
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
    }


    IEnumerator DoExplosion()
    {
        defaultGFX.SetActive(false);
        for (int i = 2; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).GetComponent<Collider>().enabled = true;
            transform.GetChild(i).GetComponent<Rigidbody>().constraints = new RigidbodyConstraints();
        }
        explosionSphere.SetActive(true);
        
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Collider>().enabled = false;
        }

        yield return new WaitForSeconds(3f);

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        Destroy(this.gameObject);
    }

}


