using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Merge_init : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] unitTiles;

    public int maxUnitLvl = 0;
    public string unitStringBackup;

    //TODO dont forget to spawn leader, he is not included in merging

    void Start()
    {
        //PlayerPrefs.SetString("units", "333333");

        unitStringBackup = PlayerPrefs.GetString("units");

        int layers = CalculateLayerNumber();
        //Debug.Log(PlayerPrefs.GetString("units") + "-->" + layers);

        //set camera zoom
        Camera.main.orthographicSize = Camera.main.orthographicSize / 3 * (layers * 2 - 1);

        string units = PlayerPrefs.GetString("units");
        //put units into tiles
        SpawnUnits(layers);
        PlayerPrefs.SetString("units", units);
    }

    private void SpawnUnits(int layers)
    {
        int colsLeft = layers;
        int colsRight = layers;
        bool half = false;
        for (int floor = 0; floor < layers; floor++)
        {
            if (half)
            {
                half = false;
                colsRight--;
            }
            else
            {
                half = true;
                colsLeft--;
            }

            for (int col = 0; col < colsLeft; col++)
            {
                SpawnFirstUnitInQueue(new Vector3Int(-col, -floor, 0));
            }
            for (int col = 0; col < colsRight; col++)
            {
                SpawnFirstUnitInQueue(new Vector3Int(col + 1, -floor, 0));
            }
        }
        colsLeft = layers-1;
        colsRight = layers;
        half = true;
        for (int floor = 1; floor < layers; floor++)
        {
            if (half)
            {
                half = false;
                colsRight--;
            }
            else
            {
                half = true;
                colsLeft--;
            }

            for (int col = 0; col < colsLeft; col++)
            {
                SpawnFirstUnitInQueue(new Vector3Int(-col, floor, 0));
            }
            for (int col = 0; col < colsRight; col++)
            {
                SpawnFirstUnitInQueue(new Vector3Int(col + 1, floor, 0));
            }
        }
    }

    private void SpawnFirstUnitInQueue(Vector3Int pos)
    {
        //Debug.Log(PlayerPrefs.GetString("units"));
        if (PlayerPrefs.GetString("units").Length == 0)
            return;

        string unitId = PlayerPrefs.GetString("units").Substring(0, 1);
        PlayerPrefs.SetString("units", PlayerPrefs.GetString("units").Substring(1));
        int id = -1;
        int.TryParse(unitId, out id);
        maxUnitLvl = Mathf.Max(maxUnitLvl, id);
        tilemap.SetTile(pos, unitTiles[id-1]);
    }

    private static int CalculateLayerNumber()
    {
        string units = PlayerPrefs.GetString("units");
        int unitsCount = units.Length - 1;
        int unitLayers = 1;
        for (; unitsCount > 0; unitLayers++)
        {
            unitsCount -= unitLayers * 6;
        }
        return unitLayers;
    }
}
