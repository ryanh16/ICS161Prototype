using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    private ParticleSystem explosion;

    private void Start()
    {
        explosion = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        RagdollToggle ragdollToggle = collision.gameObject.GetComponent<RagdollToggle>();
        if (ragdollToggle)
        {
            // put stuff here to affect the character upon hit
            Debug.Log("hit something!");
        }
        // Stuff I used in physics demo scene
        /*if (collision.gameObject.GetComponent<Rigidbody>())
        {
            explosion.transform.parent = null;
            explosion.Play();
            Destroy(explosion.gameObject, 3);
            applyForce();
            Destroy(gameObject);
        }*/
    }

    void applyForce()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(750f, transform.position, 2);
            }
        }
    }
}
