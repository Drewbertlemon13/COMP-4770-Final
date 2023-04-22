using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnit : MonoBehaviour
{
    // The base stats of our unit
    [Header("Starting Values")]
    [Tooltip("Base Health")]
    public int health = 1;
    [Tooltip("Base Damage")]
    public int damage = 10;
    [Tooltip("Team. 0. Blue, 1. Red")]
    public int team;
    [Tooltip("Unit. 0. Unit, 1. Gate, 2. Reactor")]
    public int type;
    
    // The materials for the different teams
    [Header("Materials")]
    [Tooltip("Material for the Blue Team")]
    public Material blue;
    [Tooltip("Material for the Red Team")]
    public Material red;

    // A reference to our GameData so we can add the units to the correct lists
    private GameData gameData;

    // The units set strategy
    public int strategy;

    // A flag determining if the unit is alive or not
    public bool IsAlive => health > 0;

    // A flag determining if we are reloading
    private bool shot = false;

    void Awake()
    {
        // Gets our GameData
        gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        
        // If we have a unit, add it to the unit list
        if(type != null){    
            if(type == 0)
            {
                gameData.units.Add(gameObject);
            }
        }
        else
        {
            gameData.units.Add(gameObject);
        }
    }

    // Acts as our constructor as MonoBehaviour can't have one
    public void setVariables(int health, int damage, int team, int type, int strategy)
    {
        this.health = health;
        this.damage = damage;
        this.team = team;
        this.type = type;
        this.strategy = strategy;

        if(team == 0 && type == 0){gameObject.GetComponentInChildren<MeshRenderer>().material = blue;}
        else if(team == 1 && type == 0){gameObject.GetComponentInChildren<MeshRenderer>().material = red;}
    }

    // Changes the units strategy
    public void changeStrategy(int strategy)
    {
        this.strategy = strategy;
    }

    void Update()
    {
        // If the unit dies, depending on the type, remove it from it's list
        if(!IsAlive)
        {
            if(type == 0)
            {
                gameData.units.Remove(gameObject);
            }
            else if(type == 1)
            {
                gameData.gates.Remove(gameObject);
            }
            else if(type == 2)
            {
                gameData.reactors.Remove(gameObject);
            }

            // Destroys our unit
            Destroy(gameObject);
        }
    }

    // Subtract the damage from the units health
    public void takeDamage(int damage)
    {
        health -= damage;
    }

    // Function to deal damage to an enemy
    public void attack(TestUnit enemy)
    {
        // If our enemy is alive and we're not reloading
        if(enemy.IsAlive && !shot)
        {
            shot = true;  // Take the shot
            StartCoroutine(shootInterval());  // Start Reloading
            int num = Random.Range(1, 10);  // Chance to hit
            if(num > 3)
            {
                // Create a ray shooting forward, draw it with Debug so we can see it
                Ray ray = new Ray(gameObject.transform.position + Vector3.up * 1.5f, gameObject.transform.forward * 20);
                Debug.DrawRay(gameObject.transform.position + Vector3.up * 1.5f, gameObject.transform.forward * 20, Color.red);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // If the object we hit is the enemy, deal damage
                    if(hit.transform == enemy.transform)
                    {
                        enemy.takeDamage(damage);
                        Debug.Log("Hit Enemy");
                    }
                }
            }
            else{Debug.Log("Missed");} 
        }
    }

    // The timer for reloading
    IEnumerator shootInterval()
    {
        yield return new WaitForSeconds(2);

        // Set the flag saying we have reloaded
        shot = false;
    }
}
