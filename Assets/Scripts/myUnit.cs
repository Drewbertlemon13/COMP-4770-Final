using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class myUnit : MonoBehaviour
{
    // Reference to the GameData object
    private GameObject gameData;

    // Flag to determine if the unit is attacking or not
    private bool IsAttacking = false;

    private GameObject target;  // The object the agent is targeting
    private GameData game;  // The GameData script
    private NavMeshAgent agent;  // The NavMesh Agent
    private TestUnit unit;  // The unit's script

    /*
    =====  Strategies  ===== 
        0 = Reckless Attack
        1 = Ignore Enemy, Only Win
        2 = Full Defense
    */

    void Start()
    {
        // Gets our GameData object and its script
        gameData = GameObject.FindGameObjectWithTag("GameData");
        game = gameData.GetComponent<GameData>();

        // Gets the NavMesh agent
        agent = gameObject.GetComponent<NavMeshAgent>();

        // Gets our unit script
        unit = GetComponent<TestUnit>();
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

    // The function to call deal with the attacks
    private void attack(GameObject enemy)
    {
        // Set our flag to say we're attacking
        IsAttacking = true;

        // Reset our agent path and stop to limit the drifting the agent gets sometimes
        agent.ResetPath();
        agent.isStopped = true;

        // Call the attack function from oour unit on the enemy
        unit.attack(enemy.GetComponent<TestUnit>());

        // If the enemy is dead, finish the attack
        if(!enemy.GetComponent<TestUnit>().IsAlive || Vector3.Distance(gameObject.transform.position, enemy.transform.position) > 20 || enemy == null)
        {
            // Reset our path again since the drifting is weird
            agent.ResetPath();

            // Set our flag to say we're not attacking
            IsAttacking = false;
        }
            
    }

    // Base Attack Strategy
    // Units will head towards the target
    // If they pass an enemy they can see, attack the enemy
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

        // If we find a gate in the way, attack the gate
        if(game.gates.Any(gate => gate.GetComponent<TestUnit>().team != unit.team))
        {
            foreach(GameObject gate in game.gates.Where(gate => gate.GetComponent<TestUnit>().team != unit.team))
            {
                if(Vector3.Distance(gameObject.transform.position, gate.transform.position) <= 15)
                {
                    // Look towards the gate so we can hit it
                    transform.LookAt(gate.transform);
                    attack(gate);
                }
            }
        }

        // If we reach the reactor, attack the gate
        if(game.reactors.Any(reactor => reactor.GetComponent<TestUnit>().team != unit.team))
        {
            foreach(GameObject reactor in game.reactors.Where(reactor => reactor.GetComponent<TestUnit>().team != unit.team))
            {
                if(Vector3.Distance(gameObject.transform.position, reactor.transform.position) <= 15)
                {
                    // Look towards the reactor so we can hit it
                    transform.LookAt(reactor.transform);
                    attack(reactor);
                }
            }
        }

        // Determine if we can see an enemy in range
        if(game.units.Any(enemy => enemy.GetComponent<TestUnit>().team != unit.team))
        {
            foreach(GameObject enemy in game.units.Where(enemy => enemy.GetComponent<TestUnit>().team != unit.team))
            {
                if(Vector3.Distance(gameObject.transform.position, enemy.transform.position) <= 20)
                {
                    // Gets our current angle to see if the enemy is in sight
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

    // Full Attack Strategy
    // Disregard all the enemies
    // Only care about attacking the reactor
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

        // If we find a gate in the way, attack the gate
        if(game.gates.Any(gate => gate.GetComponent<TestUnit>().team != unit.team))
        {
            foreach(GameObject gate in game.gates.Where(gate => gate.GetComponent<TestUnit>().team != unit.team))
            {
                if(Vector3.Distance(gameObject.transform.position, gate.transform.position) <= 15)
                {
                    // Look at the gate so we can hit it
                    transform.LookAt(gate.transform);
                    attack(gate);
                }
            }
        }

        // If we reach the reactor, attack it
        if(game.reactors.Any(reactor => reactor.GetComponent<TestUnit>().team != unit.team))
        {
            foreach(GameObject reactor in game.reactors.Where(reactor => reactor.GetComponent<TestUnit>().team != unit.team))
            {
                if(Vector3.Distance(gameObject.transform.position, reactor.transform.position) <= 15)
                {
                    // Look at the reactor so we can hit it
                    transform.LookAt(reactor.transform);
                    attack(reactor);
                }
            }
        }
    }

    // Full Defense
    // Find nodes to hold near the gate
    // If no nodes available, wait for one
    private void FullDefense()
    {
        // If we don't have a path and we're not attacking
        // Set the target to the node
        if(!agent.hasPath && !IsAttacking)
        {
            // Get a non-occupied node of the units team
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

        // If our node becomes occupied, find another
        else if(target != null && Vector3.Distance(gameObject.transform.position, target.transform.position) >= 5)
        {
            if(target.GetComponent<Node>().occupied == true){agent.ResetPath();}
        }

        // If we find an enemy, attack the enemy
        // No line of sight here as defenders are on guard
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
        
        // If our target isn't null and we're close to our node
        if(target != null && Vector3.Distance(gameObject.transform.position, target.transform.position) <= 2)
        {
                // Stop the agent
                agent.isStopped = true;

                // Set the node to occupied and pass this GameObject to be set in the node
                target.GetComponent<Node>().SetUnit(gameObject);
        }
    }
}
