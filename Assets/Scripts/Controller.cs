using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    [Range(0,1)]
    private float velocity = 0f;

    [SerializeField]
    [Range(0, 1)]
    private float addtionalVel = 0f;
    private float acc = 0.6f;
    private float dec = 0.6f;
    [SerializeField]
    private Animator animator;

    bool substracted = false;

    [SerializeField]
    private float speed = 6f;

    [SerializeField]
    private float turnSmoothTime = 0.1f;
    private float smoothVelocity;


    //reset anim values (after ragdolling) so player recovers in idle position
    public void resetControllerAnimValues()
    {
        animator.SetFloat("Velocity", 0);
        animator.SetFloat("AdditionalVel", 0);
        animator.SetBool("isCrouching", false);
    }


    void Update()
    {
        /*if (animator.GetBool("isCrouching"))
        {
            animator.SetFloat("Velocity", 0);
            animator.SetFloat("AdditionalVel", 0);
            velocity = 0;
            addtionalVel = 0;
        }*/

        velocity = animator.GetFloat("Velocity");
        addtionalVel = animator.GetFloat("AdditionalVel");

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) 
            && velocity < 1)
        {
            velocity += acc * Time.deltaTime;
        }

        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) 
            && velocity > 0)
        {
            if (addtionalVel > 0)
            {
                addtionalVel -= dec * Time.deltaTime;
                substracted = true;
            }
            else
            {
                velocity -= dec * Time.deltaTime;
                substracted = false;
            }
        }

        if (velocity < 0)
        {
            velocity = 0;
        }

        if (velocity >= 1)
        {
            if (Input.GetKey(KeyCode.LeftShift) && addtionalVel < 1)
            {
                addtionalVel += acc * Time.deltaTime;
            }

            if (!Input.GetKey(KeyCode.LeftShift) && addtionalVel > 0 && !substracted)
            {
                addtionalVel -= dec * Time.deltaTime;
            }
        }

        animator.SetFloat("Velocity", velocity);
        animator.SetFloat("AdditionalVel", addtionalVel);

        if (Input.GetKeyUp(KeyCode.C))
        {
            bool cur = animator.GetBool("isCrouching");
            animator.SetBool("isCrouching", !cur);
        }

        if (Input.GetMouseButtonUp(0))
        {
            animator.SetTrigger("Attack");
        }

        if (Input.GetMouseButtonUp(1))
        {
            animator.SetTrigger("Block");
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        Vector3 movement = direction * speed * Time.deltaTime;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
        }
    }
}
