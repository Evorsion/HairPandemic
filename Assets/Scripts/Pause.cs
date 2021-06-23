using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static GameObject panel = null;

    public static float OriginalTimeScale = 1;

    public bool IAmPanel;

    private void Start()
    {
        if (IAmPanel)
        {
            Pause.panel = this.gameObject;
            this.gameObject.SetActive(false);
        }
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0)
        {
            Pause.panel.SetActive(false);
            Time.timeScale = Pause.OriginalTimeScale;
        }
        else
        {
            Pause.panel.SetActive(true);
            Pause.OriginalTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
    }
}
