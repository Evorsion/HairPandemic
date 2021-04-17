using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepNoRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.localRotation = Quaternion.Euler(0,0,0);
    }
}
