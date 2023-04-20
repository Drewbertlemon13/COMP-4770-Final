using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;


public class TeamData
{
    int strategyIndex = 0;
    //int money = 0;
    List<NavMeshAgent> agents = new List<NavMeshAgent>();

    public void changeStrat(int strategy)
    {
        strategyIndex = strategy;
    }

    public void addAgent(NavMeshAgent agent)
    {
        agents.Add(agent);
    }

    public void removeAgent(NavMeshAgent agent)
    {
        agents.Remove(agent);
    }

}
