using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParentActiveFalse : MonoBehaviour
{
    public void TurnParentOff()
    {
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
