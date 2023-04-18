using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : MonoBehaviour
{
    [SerializeField]
    protected float health;

    [SerializeField]
    protected float damage;

    [SerializeField]
    protected int speed;

    [SerializeField]
    protected int strategy;

    [SerializeField]
    protected float armourResist;

    [SerializeField]
    protected float damageModifier;

    [SerializeField]
    public int team;

    [SerializeField]
    protected NavMeshAgent agent;

    [SerializeField]
    public int range;

    protected abstract void Update();

    protected abstract void Die();

    protected abstract void setTeam(int team);
    
}
