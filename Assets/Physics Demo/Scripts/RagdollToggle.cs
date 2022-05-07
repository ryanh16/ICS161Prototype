using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
    public bool ragdollEnabled;
    public BoxCollider ragdollTriggerFeet, ragdollTriggerBody, ragdollTriggerHead;
    
    public Animator animator;

    Collider[] colliders;
    Rigidbody[] limbs;
    public float corpseLifetime = 10f;  //lifetime of a ragdoll

    private void Awake() 
    {
        ragdollTriggerFeet = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        getRagdoll();
        disableRagdoll();
    }

    public void deleteRagdoll()
    {
        Destroy(gameObject);
    }
    void getRagdoll()
    {
        colliders = GetComponentsInChildren<Collider>();
        limbs = GetComponentsInChildren<Rigidbody>();
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.layer == 7)    //if collision with this layer, enables ragdoll
        {
            enableRagdoll();
        }
    }
    public void enableRagdoll()
    {
        //turns off animator first, before enabling the rest
        animator.enabled = false;

        foreach(Collider col in colliders)
        {
            col.enabled = true;
        }
        foreach(Rigidbody limb in limbs)
        {
            limb.isKinematic = false;                  //isKinematic = false allows physics to affect rigidbodies (essentially enabling them)
        }

        
        ragdollTriggerFeet.enabled = false;
        ragdollTriggerHead.enabled = false;
        ragdollTriggerBody.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;  //isKinematic = true stops physics from affecting rigidbodies (essentially disabling them)

        Destroy(gameObject, corpseLifetime);
    }

    public void disableRagdoll()
    {
        foreach(Collider col in colliders)
        {
            col.enabled = false;
        }
        foreach(Rigidbody limb in limbs)
        {
            limb.isKinematic = true;                    //isKinematic = true stops physics from affecting rigidbodies (essentially disabling them)
        }

        //turns on animator
        animator.enabled = true;
        ragdollTriggerFeet.enabled = true;
        ragdollTriggerHead.enabled = true;
        ragdollTriggerBody.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;  //isKinematic = false allows physics to affect rigidbodies (essentially enabling them)
    }
    void Update()
    {
        //testing that the ragdoll still works
        if(ragdollEnabled)
            enableRagdoll();
        else
            disableRagdoll();
    }
}
