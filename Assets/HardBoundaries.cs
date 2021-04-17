using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardBoundaries : MonoBehaviour
{
    public Transform TL;
    public Transform BR;

    public List<Transform> units = new List<Transform>();
    Transform playerUnitsParent;
    Transform enemyUnitsParent;
    void Start()
    {
        //NOTE
        //přidávání je uděláno přímo z jednotlivých jednotek,
        //každá jednotka si tudíž může zkontrolovat zda má hranice


        /*playerUnitsParent = GameObject.Find("#PlayerUnits").transform;
        enemyUnitsParent = GameObject.Find("#EnemyUnits").transform;
        
        //add player units
        foreach (Transform children in playerUnitsParent.transform)
        {
            if (children.GetComponent<Movement>())
            {
                units.Add(children);
            }
        }
        //add enemy units
        foreach (Transform group in enemyUnitsParent.transform)
        {
            foreach (Transform children in group.transform)
            {
                if (children.GetComponent<Move_by_orders>())
                {
                    units.Add(children);
                }
            }
        }*/
    }

    // Update is called once per frame
    void LateUpdate()
    {
        List<Transform> toRemove = new List<Transform>();

        foreach (Transform unit in units)
        {
            if (unit == null || unit.Equals(null))
            {
                toRemove.Add(unit);
                continue;
            }

            if (unit.position.x < TL.position.x)
            {
                unit.position = new Vector2(TL.position.x, unit.position.y);
            }
            if (unit.position.x > BR.position.x)
            {
                unit.position = new Vector2(BR.position.x, unit.position.y);
            }
            if (unit.position.y > TL.position.y)
            {
                unit.position = new Vector2(unit.position.x, TL.position.y);
            }
            if (unit.position.y < BR.position.y)
            {
                unit.position = new Vector2(unit.position.x, BR.position.y);
            }
        }

        foreach (Transform deadUnit in toRemove)
        {
            units.Remove(deadUnit);
        }
    }

    public void AddManualy(Transform unitToAdd)
    {
        if (unitToAdd.GetComponent<Movement>())
        {
            units.Add(unitToAdd);
        }
        else if (unitToAdd.GetComponent<Move_by_orders>())
        {
            units.Add(unitToAdd);
        }
        else Debug.LogWarning("Přidáná jednotka co není hráčská ani nepřátelská");
    }
}
