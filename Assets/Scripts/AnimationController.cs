using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("Walking", true);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("Running", true);
            
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("Walking", false);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("Running", false);
        }
    }
}
