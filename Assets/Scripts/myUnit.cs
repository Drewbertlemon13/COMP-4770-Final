using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class myUnit : MonoBehaviour
{
    private GameObject gameData;

    private bool IsAttacking = false;

    private GameObject target;
    private GameData game;
    private NavMeshAgent agent;
    private TestUnit unit;

    /*
        0 = Reckless Attack
        1 = Ignore Enemy, Only Win
        2 = Full Defense
    */

    void Start()
    {
        gameData = GameObject.FindGameObjectWithTag("GameData");
        agent = gameObject.GetComponent<NavMeshAgent>();
        unit = GetComponent<TestUnit>();
        game = gameData.GetComponent<GameData>();
    }

    void Update()
    {
        // Base Attack
        if(gameObject != null){
            
            //Base Attack
            if(unit.strategy == 0){BaseAttack();}

            // Full Attack
            else if(unit.strategy == 1){FullAttack();}

            // Full Defense
            else if(unit.strategy == 2){FullDefense();}
        }
    }

    private void attack(GameObject enemy)
    {
        IsAttacking = true;
        agent.ResetPath();
        agent.isStopped = true;

        unit.attack(enemy.GetComponent<TestUnit>());

        if(!enemy.GetComponent<TestUnit>().IsAlive)
        {
            agent.ResetPath();
            IsAttacking = false;
        }
            
    }

    private void BaseAttack()
    {
        // Sets path to the enemies reactor
        if(!agent.hasPath && !IsAttacking)
        {
            if(game.reactors.Any(reactor => reactor.GetComponent<TestUnit>().team != unit.team))
            {
                foreach(GameObject reactor in game.reactors.Where(reactor => reactor.GetComponent<TestUnit>().team != unit.team))
                {
                    target = reactor;
                }
                agent.SetDestination(target.transform.position);
            }
        }

        if(game.gates.Any(gate => gate.GetComponent<TestUnit>().team != unit.team))
        {
            foreach(GameObject gate in game.gates.Where(gate => gate.GetComponent<TestUnit>().team != unit.team))
            {
                if(Vector3.Distance(gameObject.transform.position, gate.transform.position) <= 15)
                {
                    transform.LookAt(gate.transform);
                    attack(gate);
                }
            }
        }

        if(game.reactors.Any(reactor => reactor.GetComponent<TestUnit>().team != unit.team))
        {
            foreach(GameObject reactor in game.reactors.Where(reactor => reactor.GetComponent<TestUnit>().team != unit.team))
            {
                if(Vector3.Distance(gameObject.transform.position, reactor.transform.position) <= 15)
                {
                    transform.LookAt(reactor.transform);
                    attack(reactor);
                }
            }
        }

        // See if we can find the enemy
        if(game.units.Any(enemy => enemy.GetComponent<TestUnit>().team != unit.team))
        {
            foreach(GameObject enemy in game.units.Where(enemy => enemy.GetComponent<TestUnit>().team != unit.team))
            {
                if(Vector3.Distance(gameObject.transform.position, enemy.transform.position) <= 20)
                {
                    Vector3 direction = (enemy.transform.position - gameObject.transform.position);
                    if(Vector3.Angle(gameObject.transform.forward, direction) < 45)
                    {
                        transform.LookAt(enemy.transform);
                        attack(enemy);
                    }
                }
            }
        }
    }

    private void FullAttack()
    {
        // Sets path to the enemies reactor
        if(!agent.hasPath && !IsAttacking)
        {
            if(game.reactors.Any(reactor => reactor.GetComponent<TestUnit>().team != unit.team))
            {
                foreach(GameObject reactor in game.reactors.Where(reactor => reactor.GetComponent<TestUnit>().team != unit.team))
                {
                    target = reactor;
                }
                agent.SetDestination(target.transform.position);
            }
        }

        if(game.gates.Any(gate => gate.GetComponent<TestUnit>().team != unit.team))
        {
            foreach(GameObject gate in game.gates.Where(gate => gate.GetComponent<TestUnit>().team != unit.team))
            {
                if(Vector3.Distance(gameObject.transform.position, gate.transform.position) <= 15)
                {
                    transform.LookAt(gate.transform);
                    attack(gate);
                }
            }
        }

        if(game.reactors.Any(reactor => reactor.GetComponent<TestUnit>().team != unit.team))
        {
            foreach(GameObject reactor in game.reactors.Where(reactor => reactor.GetComponent<TestUnit>().team != unit.team))
            {
                if(Vector3.Distance(gameObject.transform.position, reactor.transform.position) <= 15)
                {
                    transform.LookAt(reactor.transform);
                    attack(reactor);
                }
            }
        }
    }

    private void FullDefense()
    {
        // If we don't have a path and we're not attacking
        if(!agent.hasPath && !IsAttacking)
        {
            if(game.nodes.Any(node => !node.GetComponent<Node>().occupied && node.GetComponent<Node>().team == unit.team &&
                                       node.GetComponent<Node>().type == 1))
            {
                foreach(GameObject node in game.nodes.Where(node => !node.GetComponent<Node>().occupied && node.GetComponent<Node>().team == unit.team &&
                                    node.GetComponent<Node>().type == 1))
                {
                    target = node;
                }
                Debug.Log(target.transform.position);
                agent.SetDestination(target.transform.position);
            }
        }

        // If our target becomes occupied, find another
        else if(target != null && Vector3.Distance(gameObject.transform.position, target.transform.position) >= 5)
        {
            if(target.GetComponent<Node>().occupied == true){agent.ResetPath();}
        }

        if(game.units.Any(enemy => enemy.GetComponent<TestUnit>().team != unit.team))
        {
            foreach(GameObject enemy in game.units.Where(enemy => enemy.GetComponent<TestUnit>().team != unit.team))
            {
                if(Vector3.Distance(gameObject.transform.position, enemy.transform.position) <= 20)
                {
                    transform.LookAt(enemy.transform);
                    attack(enemy);
                }
            }
        }

        if(target != null && Vector3.Distance(gameObject.transform.position, target.transform.position) <= 2)
        {
                agent.isStopped = true;
                target.GetComponent<Node>().SetUnit(gameObject);
        }
    }
}
