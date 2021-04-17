using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnit : MonoBehaviour
{
    public static bool isUnit(GameObject go)
    {
        return go.GetComponent<Movement>() || go.GetComponent<Move_by_orders>();
    }
}
