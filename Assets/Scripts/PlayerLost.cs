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
            PlayerPrefs.SetString("lastLevel", SceneManager.GetActiveScene().name);

            SceneManager.LoadScene("Game over");
        }
    }
}
