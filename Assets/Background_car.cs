using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_car : MonoBehaviour
{

    public float speed;
    public float resetDistance;
    public float timeDelay;

    private Vector2 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time <= timeDelay)
            return;

        if (Vector2.Distance(transform.position, startPosition) < resetDistance)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else
        {
            transform.position = startPosition;
        }
    }
}
