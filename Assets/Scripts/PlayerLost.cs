using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLost : MonoBehaviour
{
    void Update()
    {
        if (this.transform.childCount <= 0)
        {
            //reset progress
            PlayerPrefs.SetString("units", "");
            PlayerPrefs.SetString("lastLevel", "");


            SceneManager.LoadScene("Game over");
        }
    }
}
