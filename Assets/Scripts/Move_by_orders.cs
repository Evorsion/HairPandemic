using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_by_orders : MonoBehaviour
{
    GameObject model;
    Animator animator;
    Order_machine orderMachine;

    public float movementSpeed = 6;
    Vector3 lastPos = Vector3.zero;


    private void Start()
    {
        model = this.transform.GetChild(0).gameObject;
        if (model == null) Debug.LogWarning("Model není správně nastaven");
        animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogWarning("Animator není správně nastaven");
        orderMachine = GetComponentInParent<Order_machine>();
        if (orderMachine == null) Debug.LogWarning("OrderMachine není správně nastaven");
        HardBoundaries hb = GameObject.Find("#Units").GetComponent<HardBoundaries>();
        if (hb == null) Debug.LogWarning("Jednotka nemá určené hard boundaries");
        else hb.AddManualy(this.transform);
    }

    void Update()
    {
        //turn
        Turn();

        //animation
        MovementAnimation();
    }
    private void FixedUpdate()
    {
        //move
        Move();
    }

    private void Turn()
    {
        //if (leader != null && !leader.Equals(null) && script != null && !script.Equals(null))
        if(orderMachine.moving)
        {
            //Movement.DirAndPos dap = script.GetDirectionAndPosition();
            //Vector2 direction = dap.dir + (dap.pos - (Vector2)transform.position).normalized / 10;

            Vector2 direction = orderMachine.destination - (Vector2)this.transform.position;
            direction.Normalize();

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
    }


    private void Move()
    {
        //if (leader != null && !leader.Equals(null) && script != null && !script.Equals(null))
        if (orderMachine.moving)
        {
            //Movement.DirAndPos dap = script.GetDirectionAndPosition();
            //Vector2 direction = dap.dir + (dap.pos - (Vector2)transform.position).normalized / 10;

            Vector2 direction = orderMachine.destination - (Vector2)this.transform.position;
            direction.Normalize();

            transform.Translate(direction * movementSpeed * Time.deltaTime);
        }
    }

    private void MovementAnimation()
    {
        float movedFromLastFrame = (lastPos - this.transform.position).sqrMagnitude;
        lastPos = this.transform.position;

        if (movedFromLastFrame > 0.01f)
            animator.SetBool("Moving", true);
        else
            animator.SetBool("Moving", false);
    }
}
