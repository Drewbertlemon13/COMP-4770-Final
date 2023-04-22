using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // The Spawner GameObjects
    [Header("Spawners")]
    [Tooltip("Left Blue Spawner")]
    public GameObject bSpawner1;
    [Tooltip("Right Blue Spawner")]
    public GameObject bSpawner2;
    [Tooltip("Left Red Spawner")]
    public GameObject rSpawner1;
    [Tooltip("Right Red Spawner")]
    public GameObject rSpawner2;

    // List of GameObjects for our different types
    public List<GameObject> gates;
    public List<GameObject> reactors;
    public List<GameObject> nodes;
    public List<GameObject> units;

    // Variable to hold the number of units in the unit list for each team 
    private int blueUnits;
    private int redUnits;

    // Flags to see if our respawn timer is up or not
    private bool blueRespawn = true;
    private bool redRespawn = true;

    // Strats for Blue Team
    private int bStrat1 = 0;
    private int bStrat2 = 0;

    // Strats for Red Team
    private int rStrat1 = 1;
    private int rStrat2 = 1;

    void Awake()
    {
        // Gets our lists by finding objects with the right tags
        gates = GameObject.FindGameObjectsWithTag("Gate").ToList();
        reactors = GameObject.FindGameObjectsWithTag("Reactor").ToList();
        nodes = GameObject.FindGameObjectsWithTag("Node").ToList();
    }

    void FixedUpdate()
    {
        // Reset the unit counters
        blueUnits = 0;
        redUnits = 0;

        // Check if there is a unit missing (Teams are 8 and 8)
        if(units.Count < 16)
        {
            // Goes through each unit in the unit list
            foreach(GameObject unit in units.Where(unit => unit.GetComponent<TestUnit>().team == 0))
            {
                // Everytime we come across a blue unit, add to the counter
                blueUnits += 1;
            }

            // Determine the red units by subtracting blue units from unit list length
            redUnits = units.Count - blueUnits;

            // If we're missing a blue player and respawn isn't on cooldown
            if(blueUnits < 8 && blueRespawn)
            {
                addSingleStrat(0, 0, bStrat1, bStrat2, bSpawner1, bSpawner2);
            }

            // If we're missing a red player and respawn isn't on cooldown
            else if(redUnits < 8 && redRespawn)
            {
                addSingleStrat(1, 0, rStrat1, rStrat2, rSpawner1, rSpawner2);
            }
        }
    }

    // Calls our spawner to spawn in a unit
    // Takes in team, type, and strategies to pass to the spawner
    // Takes in the Spawner GameObjects to use to call the spawn function
    public void addSingleStrat(int team, int type, int strat1, int strat2, GameObject spawner1, GameObject spawner2)
    {
        // Depending on the team, start the respawn timer
        if(team == 0)
        {
            blueRespawn = false;
        }
        else
        {
            redRespawn = false;
        }
        
        // Starts our timer coroutine
        StartCoroutine(respawn(team));

        // Get a random number 1 or 2
        switch (Random.Range(1, 3))
        {
            // If we get 1, Spawn with Strat1 and from Spawner1
            case 1:
                spawner1.GetComponent<Spawner>().Spawn(team, type, strat1);
                break;
            // If we get 2, Spawn with Strat2 and from Spawner2
            case 2:
                spawner2.GetComponent<Spawner>().Spawn(team, type, strat2);
                break;
        }
    }

    // Coroutine to keep track of the respawn time
    IEnumerator respawn(int team)
    {
        // Wait for the timer to finish
        yield return new WaitForSeconds(5);

        // Depending on the team, reset the respawn timer flag
        if(team == 0){blueRespawn = true;}
        else{redRespawn = true;}
    }
}
