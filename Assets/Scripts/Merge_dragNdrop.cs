using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Text;

[Serializable]
public class MergeRule
{
    public TileBase current;
    public TileBase next;
    public GameObject currentGameobject;

    public MergeRule(TileBase current, TileBase next, GameObject currentSprite)
    {
        this.current = current;
        this.next = next;
        this.currentGameobject = currentSprite;
    }
}

public class Merge_dragNdrop : MonoBehaviour
{

    // get the grid by GetComponent or saving it as public field
    public Grid grid;
    public Tilemap tilemap;

    bool holdingTile = false;
    TileBase holdedTile = null;
    string holdedTileName = "";
    Vector3Int ogPos;
    GameObject draggedUnit;
    bool spawnedDraggedUnit = false;

    public MergeRule[] mergeList;

    public GameObject mergeEffect;

    public Vector3 lastTouchPos;

    public Text debug;

    public int[] unitCount;

    private void Start()
    {
        int maxUnitLvl = this.GetComponent<Merge_init>().maxUnitLvl;
        string units = this.GetComponent<Merge_init>().unitStringBackup;
        unitCount = new int[maxUnitLvl];
        for (int i = 0; i < units.Length; i++)
        {
            int unitOnI = int.Parse(units[i].ToString());
            unitCount[unitOnI-1]++;
        }
    }

    private void Update()
    {
        //unit drag
        if (holdingTile)
        {
            UnitDrag();
        }

        //pickup unit
        if (!holdingTile && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            PickupUnit();
        }

        bool windows = Application.platform == RuntimePlatform.WindowsEditor;
        bool android = Application.platform == RuntimePlatform.Android;
        bool mouseUp = Input.GetMouseButtonUp(0);
        bool touchUp = Input.touchCount <= 0;

        //debug.text = "holdingTile:" + holdingTile + "\nwindows:" + windows + "\nandroid:" + android + "\nmouseUp:" + mouseUp + "\ntouchUp:" + touchUp + "\n";

        if (Input.touchCount > 0)
            lastTouchPos = Input.GetTouch(0).position;

        //debug.text += lastTouchPos;
        //debug.text += "\n" + "holdingTile && ((android && touchUp) || (windows && mouseUp))" + (holdingTile && ((android && touchUp) || (windows && mouseUp)));
        //debug.text += "\n" + "((android && touchUp) || (windows && mouseUp))" + (((android && touchUp) || (windows && mouseUp)));
        //debug.text += "\n" + "(android && touchUp)" + ((android && touchUp));
        //debug.text += "\n" + "(windows && mouseUp)" + ((windows && mouseUp));

        if (holdingTile && ((android && touchUp) || (windows && mouseUp)))
        {
            Vector3Int position = GetTilePosition();

            //if current tile is same unit --> MERGE
            if (tilemap.GetTile(position) != null && tilemap.GetTile(position).ToString() == holdedTileName)
            {
                //debug.text += "\n" + "in if";
                //MERGE
                for (int i = 0; i < mergeList.Length; i++)
                {
                    //debug.text += "\n" + "in for";
                    if (holdedTile == mergeList[i].current)
                    {
                        //last level does not merge
                        if (mergeList[i].next == null)
                        {
                            ReturnDraggedTile();
                        }
                        else
                        {
                            //debug.text += "\n" + "in if2";
                            //TODO destroy instantiated object
                            tilemap.SetTile(position, mergeList[i].next);
                            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
                            Instantiate(mergeEffect, pos, Quaternion.identity);

                            unitCount[i] -= 2;
                            unitCount[i + 1]++;

                            break;
                        }
                    }
                }

                holdedTileName = "";
                holdingTile = false;
                holdedTile = null;
                Destroy(draggedUnit);
                spawnedDraggedUnit = false;
            }
            else
            {
                ReturnDraggedTile();
            }


        }
    }

    private void ReturnDraggedTile()
    {
        //if tile is different, send tile back
        tilemap.SetTile(ogPos, holdedTile);
        holdedTileName = "";
        holdingTile = false;
        holdedTile = null;
        Destroy(draggedUnit);
        spawnedDraggedUnit = false;
    }

    private void PickupUnit()
    {
        Vector3Int position = GetTilePosition();

        //if tile is unit
        if (tilemap.GetTile(position) != null)
        {
            //hold tile
            holdingTile = true;
            holdedTile = tilemap.GetTile(position);
            holdedTileName = tilemap.GetTile(position).ToString();
            ogPos = position;
            //null original tile
            tilemap.SetTile(position, null);
        }
    }

    private void UnitDrag()
    {
        if (spawnedDraggedUnit)
        {
            draggedUnit.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        }
        else
        {
            for (int i = 0; i < mergeList.Length; i++)
            {
                if (holdedTile == mergeList[i].current)
                {
                    draggedUnit = Instantiate(mergeList[i].currentGameobject, Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10)), Quaternion.identity) as GameObject;
                    spawnedDraggedUnit = true;
                    break;
                }
            }
        }
    }

    private Vector3Int GetTilePosition()
    {
        //get tile position
        Ray ray;
#if UNITY_EDITOR
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#elif UNITY_ANDROID

        if (Input.touchCount > 0)
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        else
            ray = Camera.main.ScreenPointToRay(lastTouchPos);

#else
            Debug.LogError("Pro tuto platformu není naprogramováné dragNdrop v mergování");
#endif
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = grid.WorldToCell(worldPoint);
        return position;
    }


    public void EndMerging()
    {
        StringBuilder sb = new StringBuilder();
        for (int unitLvl = 0; unitLvl < unitCount.Length; unitLvl++)
        {
            for (int i = 0; i < unitCount[unitLvl]; i++)
            {
                sb.Append((unitLvl+1));
            }
        }
        PlayerPrefs.SetString("units", sb.ToString());
        string lastLevel = PlayerPrefs.GetString("lastLevel");
        int nextLevelNum = int.Parse(lastLevel.Substring(lastLevel.IndexOfAny("0123456789".ToCharArray()))) + 1;
        string nextLevel = lastLevel.Substring(0, lastLevel.IndexOfAny("0123456789".ToCharArray()))+ nextLevelNum;

        if (Application.CanStreamedLevelBeLoaded(nextLevel))
            SceneManager.LoadScene(nextLevel);
        else
            SceneManager.LoadScene("Win");
    }
}
