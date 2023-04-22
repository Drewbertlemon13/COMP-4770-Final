using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Node : MonoBehaviour
{
    // The variables for the node
    [Header("Node Variables")]
    [Tooltip("0.Defensive 1.Guard 2. Sniper")]
    public int type;
    [Tooltip("0. Blue 1. Red")]
    public int team;

    // A reference to the unit that is on the node
    private GameObject unit;

    // A flag to state if someone is on the node
    public bool occupied = false;

    // Set the unit and say we're occupied
    public void SetUnit(GameObject unit)
    {  
        this.unit = unit;
        occupied = true;
    }

    // If the unit is destroyed and we are occupied, reset to say we're open
    void Update()
    {
        if(unit == null && occupied == true)
        {
            occupied = false;
        }
    }
}
