using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 10;
    public float explosionPower;
    public float explosionRadius;
    public float explosionUpward;
    public float projectileSpeed;
    public Vector3 projectileScale;


    void Start()
    {
        transform.localScale = projectileScale;
        Destroy(gameObject, lifetime);
    }

    // private void OnDrawGizmosSelected() {
    // Gizmos.color = Color.red;
    // //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
    // Gizmos.DrawWireSphere (transform.position, explosionRadius);
    // }
    private void OnCollisionEnter(Collision other) 
    {
        Collider[] impacted = Physics.OverlapSphere(transform.position, explosionRadius);   //gets colliders impacted by the explosion
        for (int i = 0; i < impacted.Length; i++)
        {
            RagdollToggle ragdoll = impacted[i].GetComponent<RagdollToggle>();
            
            if(ragdoll != null)
            {
                ragdoll.enableRagdoll();
            }
        }
        impacted = Physics.OverlapSphere(transform.position, explosionRadius);  //recheck colliders after enabling ragdolls
        for (int i = 0; i < impacted.Length; i++)
        {
            Rigidbody rb = impacted[i].GetComponent<Rigidbody>();
            
            if(rb != null && !rb.gameObject.CompareTag("Projectile"))
            {
                //explosionpower/rb.mass to reduce impact with greater mass
                rb.AddExplosionForce(explosionPower, transform.position, explosionRadius, explosionUpward, ForceMode.Impulse);
            }
        }

        Destroy(gameObject);
    }
}
