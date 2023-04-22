using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Gets the Prefab of the infantry unit
    [Header("The Prefab for the unit")]
    [Tooltip("The Prefab of the infantry unit")]
    public GameObject unit;

    // Create a GameObject to hold a reference to our created unit
    private GameObject _unit;

    // Spawns in the unit taking in the team, type and strategy
    public void Spawn(int team, int type, int strategy)
    {
        // Creates the unit at the spawners location
        _unit = Instantiate(unit, transform.position, Quaternion.identity);

        // Sets the stats of the unit with the setVariables function
        _unit.GetComponent<TestUnit>().setVariables(20, 10, team, type, strategy);
    }
}
