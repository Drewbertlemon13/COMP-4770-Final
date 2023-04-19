using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("The Prefab for the unit")]
    [Tooltip("The Prefab of the infantry unit")]
    public GameObject unit;

    private GameObject _unit;

    public void Spawn(int team, int type, int strategy)
    {
        Debug.Log("Spawning Unit");
        _unit = Instantiate(unit, transform.position + transform.forward * 5, Quaternion.identity);
        _unit.GetComponent<TestUnit>().setVariables(20, 10, team, type, strategy);
    }

}
