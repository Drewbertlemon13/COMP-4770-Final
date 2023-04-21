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
    public List<GameObject> nodes;
    public List<GameObject> units;

    private int blueUnits;
    private int redUnits;

    private bool blueRespawn = true;
    private bool redRespawn = true;

    private int bStrat1 = 2;
    private int bStrat2 = 2;
    private int rStrat1 = 0;
    private int rStrat2 = 0;

    void Awake()
    {
        gates = GameObject.FindGameObjectsWithTag("Gate").ToList();
        reactors = GameObject.FindGameObjectsWithTag("Reactor").ToList();
        nodes = GameObject.FindGameObjectsWithTag("Node").ToList();
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
                addSingleStrat(0, 0, bStrat1, bStrat2, bSpawner1, bSpawner2);
            }

            else if(redUnits < 8 && redRespawn)
            {
                addSingleStrat(1, 0, rStrat1, rStrat2, rSpawner1, rSpawner2);
            }
        }
    }

    public void addSingleStrat(int team, int type, int strat1, int strat2, GameObject spawner1, GameObject spawner2)
    {
        if(team == 0)
        {
            blueRespawn = false;
        }
        else
        {
            redRespawn = false;
        }
            
        StartCoroutine(respawn(team));
        switch (Random.Range(1, 3))
        {
            case 1:
                spawner1.GetComponent<Spawner>().Spawn(team, type, strat1);
                break;
            case 2:
                spawner2.GetComponent<Spawner>().Spawn(team, type, strat2);
                break;
        }
    }

    IEnumerator respawn(int team)
    {
        yield return new WaitForSeconds(5);
        if(team == 0){blueRespawn = true;}
        else{redRespawn = true;}
    }
}
