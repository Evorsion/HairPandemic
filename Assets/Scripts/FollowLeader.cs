using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowLeader : MonoBehaviour
{
    public GameObject leader;
    public mvmnt script;
    public GameObject model;
    Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        SetLeader(leader);
    }

    private void Update()
    {
        Turn();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Turn()
    {
        if (leader != null && !leader.Equals(null) && script != null && !script.Equals(null))
        {
            Movement.DirAndPos dap = script.GetDirectionAndPosition();
            Vector2 direction = dap.dir + (dap.pos - (Vector2)transform.position).normalized / 10;
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

    Vector3 lastPos = Vector3.zero;

    private void Move()
    {
        if (leader != null && !leader.Equals(null) && script != null && !script.Equals(null))
        {
            Movement.DirAndPos dap = script.GetDirectionAndPosition();
            Vector2 direction = dap.dir + (dap.pos - (Vector2)transform.position).normalized / 10;
            direction.Normalize();

            transform.Translate(1.1f * direction * script.GetSpeed() * Time.deltaTime);

            float movedFromLastFrame = (lastPos - this.transform.position).sqrMagnitude;
            lastPos = this.transform.position;

            if (movedFromLastFrame > 0.01f)
                animator.SetBool("Moving", true);
            else
                animator.SetBool("Moving", false);
        }
    }

    public void SetLeader(GameObject newLeader)
    {
        leader = newLeader;
        if (leader != null)
        {
            script = leader.GetComponent<Movement>();
            if (script == null)
            {
                script = leader.GetComponent<RandomMovement>();
            }
        }

    }
}
