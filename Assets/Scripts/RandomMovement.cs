using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour, mvmnt
{
    public float movementSpeed = 1;
    Vector2 direction = Vector2.zero;

    public GameObject model;

    float nextDirChange = 0;
    public float dirChangeIntervalMin = 2f, dirChangeIntervalMax = 2f;
    System.Random rnd = new System.Random();

    private void Start()
    {
        model = this.transform.GetChild(0).gameObject;
        if (model == null) Debug.LogWarning("Model není správně nastaven");
    }


    private void Update()
    {
        if (nextDirChange <= Time.time)
        {
            nextDirChange = Time.time + rnd.Next((int)(dirChangeIntervalMin*100), (int)(dirChangeIntervalMax*100))/100f;
            direction = new Vector2(rnd.Next(0,100)-50, rnd.Next(0, 100)-50).normalized;
        }

        transform.Translate(direction * movementSpeed * Time.deltaTime);

        if (direction.x < 0)
        {
            model.transform.localScale = new Vector3(Mathf.Abs(model.transform.localScale.x), model.transform.localScale.y, model.transform.localScale.z);
            foreach (Transform hand in model.transform)
            {
                hand.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0);
                hand.transform.GetChild(0).transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        if (direction.x > 0)
        {
            model.transform.localScale = new Vector3(-Mathf.Abs(model.transform.localScale.x), model.transform.localScale.y, model.transform.localScale.z);
            foreach (Transform hand in model.transform)
            {
                hand.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 180);
                hand.transform.GetChild(0).transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 180);
            }
        }
    }

    public struct DirAndPos
    {
        public Vector2 dir;
        public Vector2 pos;
    }

    public Movement.DirAndPos GetDirectionAndPosition()
    {
        return new Movement.DirAndPos() { dir = direction, pos = transform.position };
    }
    public float GetSpeed()
    {
        return movementSpeed;
    }
}
