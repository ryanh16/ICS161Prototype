using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
    private bool ragdollEnabled;
    
    public Animator animator;

    public List<Collider> playerColliders;
    public List<Rigidbody> playerRigidbodies;
    public List<Collider> ragdollColliders;
    public List<Rigidbody> ragdollRigidbodies;
    public float corpseLifetime = 10f;  //lifetime of a ragdoll

    RagdollRecovery ragdollRecovery;
    ThirdPersonMovement playerController;

    private void Awake() 
    {
        // ragdollTriggerFeet = GetComponent<BoxCollider>();
        ragdollRecovery = GetComponentInChildren<RagdollRecovery>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<ThirdPersonMovement>();

        sortCollidersAndRigidbodies();
        disableRagdoll();
    }

    public void deleteRagdoll()
    {
        Destroy(gameObject);
    }

    public bool isRagdolled()
    {
        return ragdollEnabled;
    }

    public Vector3 getRagdollLocation()
    {
        //gets location of ragdoll
        return animator.GetBoneTransform(HumanBodyBones.Head).position;
    }

    //sorts colliders and rigidbodies, separating animated player from ragdolled player
    void sortCollidersAndRigidbodies()
    {
        Collider[] allColliders = GetComponentsInChildren<Collider>();
        Rigidbody[] allRigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Collider col in allColliders)
        {
            /*
            if collider is not from parent transform, then it's considered a ragdoll collider. 
            else considered player collider
            */
            if(col.transform != transform)
            {
                ragdollColliders.Add(col);
            }
            else
            {
                playerColliders.Add(col);
            }
        }

        foreach(Rigidbody rb in allRigidbodies)
        {
            /*
            if rb is not from parent transform, then it's considered a ragdoll rb. 
            else considered player rb
            */
            if(rb.transform != transform)
            {
                ragdollRigidbodies.Add(rb);
            }
            else
            {
                playerRigidbodies.Add(rb);
            }
        }
    }
    // private void OnCollisionEnter(Collision col)
    // {
    //     //if collision with this object from this layer or tag, enables ragdoll
    //     // Debug.Log($"tag: {col.gameObject.CompareTag("Projectile")}, layer: {col.gameObject.layer}");
    //     if(col.gameObject.CompareTag("Projectile")  || col.gameObject.layer == 7)
    //     {
    //         enableRagdoll();
    //     }
    // }

    void togglePhysics(bool enabled, List<Collider> colliderList, List<Rigidbody> rbList)
    {
        /*
        if physics enabled, turns colliders on, sets rb kinematics to false (letting physics affect all objects in the list)
        else does the opposite
        */
        foreach(Collider col in colliderList)
        {
            col.enabled = enabled;
        }
        foreach(Rigidbody rb in rbList)
        {
            rb.isKinematic = !enabled;
        }
    }

    public void enableRagdoll()
    {
        ragdollEnabled = true;
        playerController.enabled = false;
        //turns off animator first, before enabling the rest
        animator.enabled = false;

        // //disables charactercontroller
        // CharacterController characterController = GetComponent<CharacterController>();
        // characterController.enabled = false;
        // playerController.enabled = false;

        //turns physics off for player before enabling ragdoll to prevent crazy collisions
        togglePhysics(false, playerColliders, playerRigidbodies);
        togglePhysics(true, ragdollColliders, ragdollRigidbodies);


        StartCoroutine(beginRagdollRecovery(2f));

        //if ragdoll corpse wants to be destroyed after a set amount of time
        if(corpseLifetime > 0)
        {
            Destroy(gameObject, corpseLifetime);
        }
    }

    public void disableRagdoll()
    {
        ragdollEnabled = false;
        playerController.enabled = true;
        //turns physics off for ragdoll before enabling player to prevent crazy collisions
        togglePhysics(false, ragdollColliders, ragdollRigidbodies);
        togglePhysics(true, playerColliders, playerRigidbodies);
        

        //turns on animator after ragdoll disabled
        animator.enabled = true;


        // CharacterController characterController = GetComponent<CharacterController>();
        // characterController.enabled = true;
        // playerController.enabled = true;
    }
    void Update()
    {
        // //testing that the ragdoll still works
        // if(ragdollEnabled)
        //     enableRagdoll();
        // // else
        // //     disableRagdoll();
    }

    IEnumerator beginRagdollRecovery(float time)
    {
        yield return new WaitForSeconds(time);
        ragdollRecovery.state = RagdollRecovery.RagdollState.ragdolled;
        // Debug.Log("State changed to blending");
    }
}
