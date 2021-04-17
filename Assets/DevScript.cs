using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevScript : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Time.timeScale++;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Time.timeScale--;
        }
        Time.timeScale = Mathf.Clamp(Time.timeScale, 1, 8);
    }

    public void FasterTime()
    {
        Time.timeScale++;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 1,8);
    }
}
