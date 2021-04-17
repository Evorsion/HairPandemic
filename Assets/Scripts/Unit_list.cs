using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Unit
{
    public string name;
    public GameObject prefab;
}

[Serializable]
public class PlayerUnit : Unit
{
}

[Serializable]
public class EnemyUnit : Unit
{

}

[Serializable]
public class DeadUnit : Unit
{

}

[Serializable]
public class UnitLevel
{
    public PlayerUnit player;
    public EnemyUnit enemy;
    public DeadUnit dead;
}

public class Unit_list : MonoBehaviour
{
    public UnitLevel[] units;
    public GameObject changeFractionEffect;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public PlayerUnit GetPlayerUnit(int level)
    {
        return units[level].player;
    }

    public EnemyUnit GetEnemyUnit(int level)
    {
        return units[level].enemy;
    }

    public DeadUnit GetDeadUnit(int level)
    {
        return units[level].dead;
    }

    public GameObject GetChangeFractionEffect()
    {
        return changeFractionEffect;
    }
}


