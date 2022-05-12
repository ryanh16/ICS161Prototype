using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform firePos;
    [SerializeField]
    private GameObject barrel;
    [SerializeField]
    private float rotateSpeed = 3f;

    // Update is called once per frame
    void Update()
    {
        float eularZ = barrel.transform.eulerAngles.z;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject instance = Instantiate(bullet, firePos.position, firePos.rotation);

            instance.GetComponent<Rigidbody>().velocity = -firePos.up * 20;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            
            if (eularZ > 315 || eularZ <= 45 || eularZ == 0)
            {
                barrel.transform.Rotate(rotateSpeed * -Vector3.forward);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (eularZ >= 315 || eularZ < 40 || eularZ == 0)
            {
                barrel.transform.Rotate(rotateSpeed * Vector3.forward);
            }
        }
    }
}
