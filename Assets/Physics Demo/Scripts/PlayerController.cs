using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [Header("Movement")]
    float moveSpeed;
    public float walkSpeed, runSpeed;
    Transform orientation;
    float horizontal, vertical;   //horizontal, vertical input
    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Shooting")]
    Camera playerCam;
    Vector3 projectileDest;
    public float maxProjectileDist;
    public GameObject projectile;
    public Transform firePosition;  //position where projectile was fired

    public float fireCooldown = 3;  //cooldown for firing projectile
    [SerializeField] float cooldownTimer = 0;

    private void Awake() 
    {
        playerCam = GameObject.Find("playerCam").GetComponent<Camera>();
        orientation = transform.Find("playerOrientation");
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        checkSprint();
    }

    public void refreshFireCooldown()
    {
        cooldownTimer = 0f;
    }

    void checkFire()
    {
        if(Input.GetButton("Fire1") && cooldownTimer <= 0)
        {
            // Debug.Log("Projectile fired");
            shootProjectile();
            cooldownTimer = fireCooldown;
        }
        else if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
    void checkMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void checkSprint()
    {
        if(Input.GetButton("Sprint"))
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

    }
    void Update()
    {
        if(CameraController.cursorLocked)
        {
            checkFire();
        }
        checkMovement();
        checkSprint();
        checkMaxSpeed();

    }
    private void FixedUpdate() 
    {
        move();
    }
    void move()
    {
        moveDirection = orientation.forward * vertical + orientation.right * horizontal;

        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }

    void checkMaxSpeed()    //limits maxspeed to movespeed
    {
        Vector3 currentVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(currentVel.magnitude > moveSpeed)
        {
            Vector3 cappedVel = currentVel.normalized * moveSpeed;
            rb.velocity = new Vector3(cappedVel.x, rb.velocity.y, cappedVel.z);
        }
    }

    void shootProjectile()
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit))
        {
            projectileDest = hit.point;
        }
        else
        {
            projectileDest = ray.GetPoint(maxProjectileDist);
        }

        instantiateProjectile();

    }

    void instantiateProjectile()
    {
        float projectileSpeed = projectile.GetComponent<Projectile>().projectileSpeed;  //gets speed from projectile to use when calculating velocity
        GameObject obj = Instantiate(projectile, firePosition.position, Quaternion.identity) as GameObject;
        obj.GetComponent<Rigidbody>().velocity = (projectileDest - firePosition.position).normalized * projectileSpeed; //velocity towards ray
    }

}
