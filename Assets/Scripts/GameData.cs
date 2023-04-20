using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [Header("Spawners")]
    [Tooltip("Left Blue Spawner")]
    public GameObject bSpawner1;
    [Tooltip("Right Blue Spawner")]
    public GameObject bSpawner2;
    [Tooltip("Left Red Spawner")]
    public GameObject rSpawner1;
    [Tooltip("Right Red Spawner")]
    public GameObject rSpawner2;

    private GameObject[] gateArr;
    private GameObject[] blueTeam;
    private GameObject[] redTeam;
    private GameObject[] reactorArr;

    public List<GameObject> gates;
    public List<GameObject> reactors;
    public List<GameObject> units;

    private int blueUnits;
    private int redUnits;

    private bool blueRespawn = true;
    private bool redRespawn = true;

    void Awake()
    {
        gates = GameObject.FindGameObjectsWithTag("Gate").ToList();
        reactors = GameObject.FindGameObjectsWithTag("Reactor").ToList();
    }

    void FixedUpdate()
    {
        blueUnits = 0;
        redUnits = 0;
        if(units.Count < 16)
        {
            foreach(GameObject unit in units.Where(unit => unit.GetComponent<TestUnit>().team == 0))
            {
                blueUnits += 1;
            }
            redUnits = units.Count - blueUnits;
            if(blueUnits < 8 && blueRespawn)
            {
                blueRespawn = false;
                StartCoroutine(respawn(blueRespawn));
                int num = Random.Range(1, 3);
                if(num == 1)
                {
                    bSpawner1.GetComponent<Spawner>().Spawn(0, 0, 0);
                }
                else
                {
                    bSpawner2.GetComponent<Spawner>().Spawn(0, 0, 0);
                }
            }
            if(redUnits < 8 && redRespawn == true)
            {
                redRespawn = false;
                StartCoroutine(respawn(redRespawn));
                int num = Random.Range(1, 3);
                if(num == 1)
                {
                    rSpawner1.GetComponent<Spawner>().Spawn(1, 0, 0);
                }
                else
                {
                    rSpawner2.GetComponent<Spawner>().Spawn(1, 0, 0);
                }
            }
        }
    }

    IEnumerator respawn(bool respawn)
    {
        yield return new WaitForSeconds(5);
        if(respawn == redRespawn){redRespawn = true;}
        else if (respawn == blueRespawn){blueRespawn = true;}
    }
}
