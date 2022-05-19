using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        RagdollToggle ragdollToggle = collision.gameObject.GetComponent<RagdollToggle>();
        float vel = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

        // if the collided object is a ragdoll character and is running (speed > 0)
        // then this script will enable the RagdollToggle and allows
        if (ragdollToggle && vel > 0)
        {
            ragdollToggle.enableRagdoll();
        }

    }
}
