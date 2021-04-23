using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_spawner : MonoBehaviour
{

    Unit_list listOfUnits;
    GameObject playerUnitsDir;

    private void Start()
    {
        listOfUnits = GameObject.Find("#UnitAuthority").GetComponent<Unit_list>();
        playerUnitsDir = GameObject.Find("#PlayerUnits");
        string units = PlayerPrefs.GetString("units");
        Vector2 spawnPos = playerUnitsDir.transform.GetChild(0).transform.position;

        Destroy(playerUnitsDir.transform.GetChild(0).gameObject);

        HardBoundaries hb = GameObject.Find("#Units").GetComponent<HardBoundaries>();

        for (int unit = 0; unit < units.Length; unit++)
        {
            int lvl = int.Parse(units[unit].ToString())-1;
            var newUnit = Instantiate(listOfUnits.units[lvl].player.prefab, spawnPos+new Vector2(Random.Range(-0.1f,0.1f), Random.Range(-0.1f, 0.1f)), Quaternion.identity, playerUnitsDir.transform)as GameObject;
            newUnit.name = listOfUnits.units[lvl].player.name;
        }
    }
}
