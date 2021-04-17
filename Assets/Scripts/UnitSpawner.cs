using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject enemyUnitsGroup;
    public GameObject leaderPrefab;
    public GameObject unitPrefab;
    public const int defaultArmySize = 5;

    private void Start()
    {
        SpawnWave(5);
        InvokeRepeating("SpawnWave", 30, 30);
    }

    public void SpawnWave()
    {
        //spawn leader
        GameObject leader = Instantiate(leaderPrefab, this.transform.position, Quaternion.identity) as GameObject;
        Fraction.Team leaderTeam = leader.GetComponent<Fraction>().team;
        leader.transform.parent = enemyUnitsGroup.transform;

        //spawn army
        for (int i = 0; i < defaultArmySize; i++)
        {
            GameObject unit = Instantiate(unitPrefab, this.transform.position, Quaternion.identity) as GameObject;
            unit.GetComponent<FollowLeader>().SetLeader(leader);
            unit.GetComponent<Fraction>().team = leaderTeam;
            unit.transform.parent = enemyUnitsGroup.transform;
        }
    }

    public void SpawnWave(int size = defaultArmySize)
    {
        //spawn leader
        GameObject leader = Instantiate(leaderPrefab, this.transform.position, Quaternion.identity) as GameObject;
        Fraction.Team leaderTeam = leader.GetComponent<Fraction>().team;
        leader.transform.parent = enemyUnitsGroup.transform;

        //spawn army
        for (int i = 0; i < size; i++)
        {
            GameObject unit = Instantiate(unitPrefab, this.transform.position, Quaternion.identity) as GameObject;
            unit.GetComponent<FollowLeader>().SetLeader(leader);
            unit.GetComponent<Fraction>().team = leaderTeam;
            unit.transform.parent = enemyUnitsGroup.transform;
        }
    }
}
