using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    RagdollToggle ragdollToggle;

    [SerializeField]
    private Vector3 normalOffset;
    [SerializeField]
    private Vector3 ragdollOffset;

    void Start()
    {
        ragdollToggle = player.GetComponent<RagdollToggle>();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if(!ragdollToggle.isRagdolled())
        {
            transform.position = player.transform.position + normalOffset;
        }
        //if in ragdoll, follow ragdoll position
        else
        {
            transform.position = ragdollToggle.getRagdollLocation() + ragdollOffset;
        }
        
    }
}
