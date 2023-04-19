using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnit : MonoBehaviour
{
    /*
        0 = Reckless Attack
        1 = Ignore Enemy, Only Win
    */

    [Header("Starting Values")]
    [Tooltip("Base Health")]
    public int health = 1;
    [Tooltip("Base Damage")]
    public int damage = 10;
    [Tooltip("Team. 0. Blue, 1. Red")]
    public int team;
    [Tooltip("Unit. 0. Unit, 1. Gate, 2. Reactor")]
    public int type;
    
    [Header("Materials")]
    [Tooltip("Material for the Blue Team")]
    public Material blue;
    [Tooltip("Material for the Red Team")]
    public Material red;

    private GameData gameData;
    public int strategy = 0;
    public bool IsAlive => health > 0;
    private bool shot = false;

    void Awake()
    {
        gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        gameData.units.Add(gameObject);
    }

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

    public void changeStrategy(int strategy)
    {
        this.strategy = strategy;
    }

    // Update is called once per frame
    void Update()
    {
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
            Destroy(gameObject);
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
    }

    public void attack(TestUnit enemy)
    {
        if(enemy.IsAlive && !shot)
        {
            shot = true; // Take the shot
            StartCoroutine(shootInterval());
            int num = Random.Range(1, 10); // Chance to hit
            if(num > 3)
            {
                Ray ray = new Ray(gameObject.transform.position + Vector3.up * 1.5f, gameObject.transform.forward * 20);
                Debug.DrawRay(gameObject.transform.position + Vector3.up * 1.5f, gameObject.transform.forward * 20, Color.red);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
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

    IEnumerator shootInterval()
    {
        yield return new WaitForSeconds(2);
        shot = false;
    }
}
