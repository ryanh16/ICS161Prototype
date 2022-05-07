using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public PlayerController player; //for setting cooldown values
    public GameObject ragdollPrefab;
    GameObject spawnHolder;
    public Transform[] spawnPoints;
    public GameObject projectile;   //for editing projectile prefab


    private void Awake() 
    {
        weakProjectile();   //sets default as weak projectile
        spawnHolder = GameObject.Find("RagdollSpawns");
        spawnPoints = spawnHolder.transform.GetComponentsInChildren<Transform>();
        respawnRagdolls();
    }

    void changeFireCooldown(float maxCooldown)
    {
        player.fireCooldown = maxCooldown;
        player.refreshFireCooldown();
    }
    void changeProjectileAttributes(Vector3 scale, float power, float radius, float upward, float speed = 1f, float lifetime = 3f)
    {
        projectile.GetComponent<Projectile>().projectileScale = scale;
        projectile.GetComponent<Projectile>().explosionPower = power;
        projectile.GetComponent<Projectile>().explosionRadius = radius;
        projectile.GetComponent<Projectile>().explosionUpward = upward;
        projectile.GetComponent<Projectile>().projectileSpeed = speed;
        projectile.GetComponent<Projectile>().lifetime = lifetime;


    }
    public void weakProjectile()
    {
        changeProjectileAttributes( new Vector3(0.25f, 0.25f, 0.25f), 10, 1f, 0.05f, 15f, 2f);
        changeFireCooldown(0.3f);
    }

    public void lessWeakProjectile()
    {
        changeProjectileAttributes(new Vector3(0.4f, 0.4f, 0.4f), 90, 1f, 0.15f, 5f, 3f);
        changeFireCooldown(0.75f);
    }

    public void mediumProjectile()
    {
        changeProjectileAttributes(new Vector3(0.5f, 0.5f, 0.5f), 90, 2.5f, 0.25f, 3f, 5f);
        changeFireCooldown(1f);
    }

    public void strongProjectile()
    {
        changeProjectileAttributes(new Vector3(0.65f, 0.65f, 0.65f), 150, 8f, 0.35f, 2f, 8f);
        changeFireCooldown(2f);
    }

    public void veryStrongProjectile()
    {
        changeProjectileAttributes(new Vector3(0.8f, 0.8f, 0.8f), 450, 15f, 0.65f, 1f, 20f);
        changeFireCooldown(5f);
    }


    public void respawnRagdolls()
    {
        foreach(Transform spawn in spawnPoints)
        {
            if(spawn.gameObject.name == "RagdollSpawns")
            {
                continue;
            }
            //will delete previous ragdolls if there are any
            if(spawn.gameObject.GetComponentInChildren<RagdollToggle>() != null)
            {
                spawn.gameObject.GetComponentInChildren<RagdollToggle>().deleteRagdoll();
                //Debug.Log("Deleted duplicate ragdolls");
            }
            GameObject ragdoll = Instantiate(ragdollPrefab, spawn.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            ragdoll.transform.parent = spawn.transform; //parents ragdolls to their spawns
        }
    }
    public void reloadScene()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
        //Debug.Log("Quit Game");
    }
}
