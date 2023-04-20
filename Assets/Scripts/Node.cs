using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Node : MonoBehaviour
{
    [Header("Node Variables")]

    [Tooltip("0.Defensive 1.Guard 2. Sniper")]
    public int type; // 0. Defensive 1. Guard 2. Sniper
    
    [Tooltip("0. Blue 1. Red")]
    public int team; // 0. Blue, 1. Red

    private GameObject unit;
    public bool occupied = false;

    public void SetUnit(GameObject unit)
    {  
        this.unit = unit;
        occupied = true;
    }

    void FixedUpdate()
    {
        if(unit == null && occupied == true)
        {
            occupied = false;
        }
    }
}
