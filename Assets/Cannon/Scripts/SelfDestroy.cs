using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    private float v;

    // Update is called once per frame
    void Update()
    {
        v = GetComponent<Rigidbody>().velocity.magnitude;
        if (v > 50)
        {
            Destroy(gameObject);
        }
    }
}
