using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void OnCollisionEnter(Collision collision)
    {   
        RagdollToggle ragdollToggle = collision.gameObject.GetComponent<RagdollToggle>();
        // UPDATED 5/22: now the RagdollToggle will only be enabled if the player's
        // "AdditionalVel" in the animator is greater than 0.5
        float vel = animator.GetFloat("AdditionalVel");


        

        // if the collided object is a ragdoll character and is running (speed > 0)
        // then this script will enable the RagdollToggle and allows
        if (ragdollToggle && vel >= 0.25f)
        {
            ragdollToggle.enableRagdoll();

            List<Rigidbody> ragdollLimbs = ragdollToggle.getRagdollLimbs();
            foreach(Rigidbody rb in ragdollLimbs)
            {
                rb.AddForce(collision.transform.forward * 20f * vel, ForceMode.Impulse);
            }
        }

    }
}
