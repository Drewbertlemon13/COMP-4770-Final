using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Infantry : Unit
{
    void Awake()
    {
        // Set our base stats for infantry
        health = 100;
        damage = 5;
        range = 5;
        speed = 1;
        strategy = 0;
        armourResist = 1;
        damageModifier = 1;

        // Gets the agent component
        agent = GetComponent<NavMeshAgent>();
    }

    // Update for our agent
    protected override void Update()
    {
        
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    protected override void setTeam(int team)
    {
        this.team = team;
    }
}
