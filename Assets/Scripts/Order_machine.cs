using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order_machine : MonoBehaviour
{
    public bool moving = false;
    public bool waiting = false;
    public Vector2 destination;
    public Transform topLeftBoundary;
    public Transform bottomRightBoundary;

    public float waitTime = 2;
    public float arivalPrecision = 1;

    private void Start()
    {
        destination = GenerateDestination();
        moving = true;
    }

    private void Update()
    {
        //if no unit, destroy itself, has two boundaries
        bool lastTwoAreBoundaries = false;
        if (this.transform.childCount == 2)
        {
            lastTwoAreBoundaries = this.transform.GetChild(0).name == "TL" || this.transform.GetChild(0).name == "BR" || this.transform.GetChild(1).name == "TL" || this.transform.GetChild(1).name == "BR";
        }
        
        //print(lastTwoAreBoundaries);
        if (this.transform.childCount <= 0 || (this.transform.childCount <= 2 && lastTwoAreBoundaries))
        {
            //print("Jsem v ifu");
            Destroy(this.gameObject);
        }

        SetDestinationWhenNeeded();
    }



    Vector2 groupAveragePos;
    private void SetDestinationWhenNeeded()
    {
        if (moving)
        {
            if (this.transform.childCount > 0)
            {
                //detect arival to destination --> moving = false
                groupAveragePos = Vector2.zero;
                int markerCount = 0;
                foreach (Transform child in this.transform)
                {
                    if (child.name == "TL" || child.name == "BR")
                    {
                        markerCount++;
                        continue;
                    }
                        
                    groupAveragePos += (Vector2)child.position;
                }
                groupAveragePos /= this.transform.childCount-markerCount;

                if (Vector2.Distance(groupAveragePos, destination) <= arivalPrecision)
                {
                    moving = false;
                }
            }
        }
        else
        {
            if (waiting == false)
            {
                StartCoroutine(WaitThanChangeDestination());
                waiting = true;
            }
        }
    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groupAveragePos, 1);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(destination, 1);
    }*/

    IEnumerator WaitThanChangeDestination()
    {
        yield return new WaitForSeconds(waitTime);

        //generate new destination
        destination = GenerateDestination();
        moving = true;
        waiting = false;
    }

    private Vector2 GenerateDestination()
    {
        float destinationX = Random.Range(topLeftBoundary.transform.position.x, bottomRightBoundary.transform.position.x);
        float destinationY = Random.Range(topLeftBoundary.transform.position.y, bottomRightBoundary.transform.position.y);
        Vector2 newDestination = new Vector2(destinationX, destinationY);
        return newDestination;
    }
}
