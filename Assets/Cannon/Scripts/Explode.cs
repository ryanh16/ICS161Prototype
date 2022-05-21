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
            applyForce();
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

    /*
    checks for ragdoll scripts within each detected object (used in the overlap sphere)
    then enables ragdolls for them to allow for force to be applied to them
    */
    void checkForRagdolls(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            RagdollToggle ragdoll = colliders[i].GetComponent<RagdollToggle>();
            if(ragdoll != null)
            {
                // Debug.Log("Enabling ragdoll");
                ragdoll.enableRagdoll();
            }
        }
    }
    void applyForce()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2);

        checkForRagdolls(colliders);
        //after enabling ragdolls, refresh colliders array to find new colliders
        colliders = Physics.OverlapSphere(transform.position, 2);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
            if (rb)
            {
                // Debug.Log($"Added explosion force to {rb.gameObject}");
                rb.AddExplosionForce(3000f, transform.position, 2);
            }
        }
    }
}
