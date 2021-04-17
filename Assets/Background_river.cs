using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_river : MonoBehaviour
{
    public float segmentLength;
    public float speed;


    private Vector2 startPosition;
    void Start()
    {

        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, startPosition) < segmentLength)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else
        {
            transform.position = startPosition;
        }
    }
}
