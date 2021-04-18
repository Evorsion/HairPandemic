using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWon : MonoBehaviour
{

    void Update()
    {
        if (this.transform.childCount <= 0)
        {
            string units = "";

            foreach (Transform t in GameObject.Find("#PlayerUnits").transform)
            {
                if (!t.gameObject.activeInHierarchy) continue;

                string number = t.name.Substring(t.name.IndexOf('.')+1);
                int lvl = 0;
                int.TryParse(number, out lvl);

                if (lvl != 0)
                {
                    units += lvl;
                }
            }

            PlayerPrefs.SetString("units", units);
            PlayerPrefs.SetString("lastLevel", SceneManager.GetActiveScene().name);

            Debug.Log("Jednotky:"+units);

            SceneManager.LoadScene("Merge");

        }
    }
}
