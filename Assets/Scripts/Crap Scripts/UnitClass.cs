using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitClass : MonoBehaviour
{
    // Variables for this agent
    public NavMeshAgent agent {get; private set; }
    public float health { get; private set; }
    public int speed { get; private set; }
    public int strategy { get; private set; }
    public float damage {get; private set; }

    /// <summary>
    /// The microbe that this microbe is moving towards to either eat or mate with.
    /// </summary>
    private UnitClass currTarget;
    
    
    private float armourResist = 1;
    private float damageModifier = 1;
    public bool IsAlive => health < 0;
    
    // Take damage
    public void takeHit(int damage)
    {
        health -= damage * armourResist;
    }

    // Check if we shot the guy or not
    public bool shoot(NavMeshAgent target)
    {
        if(4 < Random.Range(0.0f, 10.0f))
        {
            return true;
        }
        return false;
    }

    // Attacks the agent that is passed
    public bool attack(NavMeshAgent target)
    {
        // Shoot action to hit
        if(shoot(target))
        {
            // target.takeHit(damage*damageModifier);
            return true;
        }
        return false;
    }

    public void SetTargetUnit(UnitClass target)
    {
        currTarget = target;
    }

    public void RemoveTargetUnit()
    {
        currTarget = null;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}

