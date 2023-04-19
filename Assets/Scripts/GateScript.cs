using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    [Header("Starting Values")]
    [Tooltip("The Health of the Gate")]
    public float health;
    [Tooltip("Team of the Gate. 0. Blue, 1. Red")]
    public int team;

    bool IsAlive => health > 0;
    
    void Update()
    {
        if(!IsAlive)
        {
            Destroy(gameObject);
        }
    }
}
