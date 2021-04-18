using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface mvmnt
{
    Movement.DirAndPos GetDirectionAndPosition();
    float GetSpeed();
}

public class Movement : MonoBehaviour, mvmnt
{
    //movement
    bl_Joystick Joystick;
    public float movementSpeed = 6;
    Vector2 direction = Vector2.zero;

    //turning
    GameObject model;

    //animation
    Animator animator;
    Vector3 lastPos = Vector3.zero;     //determines if unit has moved

    //physic fix
    Rigidbody2D rb;

    private void Start()
    {
        Joystick = GameObject.Find("#Joystick").GetComponent<bl_Joystick>();
        if (Joystick == null) Debug.LogWarning("Joystick není správně nastaven");
        model = this.transform.GetChild(0).gameObject;
        if (model == null) Debug.LogWarning("Model není správně nastaven");
        animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogWarning("Animator není správně nastaven");
        rb = this.GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogWarning("Rigidbody není správně nastaven");
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

        //Physics fix
        rb.velocity = Vector3.zero;
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

    private void Turn()
    {
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

    private void Move()
    {
        float horizontal = Joystick.Horizontal;
        float vertical = Joystick.Vertical;

        if (Mathf.Abs(horizontal) < 0.01f)
            horizontal = 0;
        if (Mathf.Abs(vertical) < 0.01f)
            vertical = 0;

        //no movement --> regroup
        if (horizontal == 0 && vertical == 0)
        {
            Regroup();
        }

        direction = new Vector2(horizontal, vertical).normalized;

        transform.Translate(direction * movementSpeed * Time.deltaTime);
    }

    private void Regroup()
    {
        //find average position
        Vector2 averagePos = GetAveragePosition();

        //move towards that position
        transform.Translate((averagePos - (Vector2)this.transform.position)*Time.deltaTime);
    }

    private Vector2 GetAveragePosition()
    {
        //get parent
        Transform parentToAllUnits = this.transform.parent;

        //sum positions of all units
        Vector2 posSum = Vector2.zero;
        foreach (Transform unit in parentToAllUnits.transform)
        {
            posSum += (Vector2)unit.transform.position;
        }
        
        return posSum / parentToAllUnits.transform.childCount;
    }

    public struct DirAndPos
    {
        public Vector2 dir;
        public Vector2 pos;
    }

    public DirAndPos GetDirectionAndPosition()
    {
        return new DirAndPos() { dir = direction, pos = transform.position };
    }

    public float GetSpeed()
    {
        return movementSpeed;
    }
}
