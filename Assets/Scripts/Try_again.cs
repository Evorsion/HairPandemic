using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Try_again : MonoBehaviour
{
    public void TryAgain()
    {
        string thisLevel = PlayerPrefs.GetString("lastLevel");
        if (Application.CanStreamedLevelBeLoaded(thisLevel))
            SceneManager.LoadScene(thisLevel);
    }
}
