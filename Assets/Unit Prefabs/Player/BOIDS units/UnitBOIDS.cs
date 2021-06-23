using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBOIDS : MonoBehaviour
{
    //UNIT VARIABLES
    //movement
    bl_Joystick Joystick;
    //public float movementSpeed = 6;       //merged with BOIDS speed
    //Vector2 direction = Vector2.zero;     //merged with BOIDS velocity

    //turning
    GameObject model;

    //animation
    Animator animator;
    Vector3 lastPos = Vector3.zero;         //determines if unit has moved

    //physic fix
    Rigidbody2D rb;








    //BOIDS VARIABLES

    public Vector2 boundaries;
    public Vector2 velocity;


    //BOIDS - algorithm
    //boid - object behaving like bird

    [Header("Movement coeficients")]
    //---------------------------
    [Tooltip("Movement speed of all boids")]
    public float speed;     //TODO speed range
    //---------------------------
    [Tooltip("Percent of impact of random movement on velocity")]
    [Range(0, 1)]
    public float randomnessInfluence;
    //---------------------------
    [Tooltip("Percent of impact of input from joystick on velocity")]
    [Range(0, 1)]
    public float joystickInfluence;


    //---------------------------
    [Header("BOIDS behaviour")]
    //---------------------------
    [Tooltip("Percent of impact of BOIDS steering calculations on velocity")]
    [Range(0, 1)]
    public float boidEffect;
    //---------------------------
    [Tooltip("Takes only boids in this range to consideration")]
    public float localRadius;
    //---------------------------
    [Tooltip("Steers away from other boids in range")]
    [Range(0, 1)]
    public float separationCoeficient;
    //---------------------------
    [Tooltip("Steers in same sirection as boids in range")]
    [Range(0, 1)]
    public float alignementCoeficient;
    //---------------------------
    [Tooltip("Steers closer to other boids in range")]
    [Range(0, 1)]
    public float cohesionCoeficient;


    private void Start()
    {
        //UNIT INICIALIZATION
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




        //BOIDS INICIALIZATION
        this.gameObject.name = Random.Range(0, 10000000) + " " + this.gameObject.name;        //unique name for diferentiation from self

        float xv = Random.Range(-boundaries.x, boundaries.x);
        float yv = Random.Range(-boundaries.y, boundaries.y);
        velocity = new Vector2(xv, yv).normalized;
        //this.name = "Boid" + Random.Range(0, 1000000);                //units has their own naming convention
        //float x = Random.Range(-boundaries.x, boundaries.x);          //unit is given fixed initial position
        //float y = Random.Range(-boundaries.y, boundaries.y);          //unit is given fixed initial position
        //this.transform.position = new Vector2(x, y);
    }

    /*private void Update()
    {
        //move
        transform.Translate(velocity * Time.deltaTime * speed);
        //LoopAroundBox();


    }*/

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

    #region BOIDS
    private void CalculateAlteredVelocity(Vector2 joystickInput)
    {
        List<Transform> otherLocalBoids = new List<Transform>();
        List<Transform> otherLocalBoidsDouble = new List<Transform>();
        foreach (Transform boid in this.transform.parent.transform)
        {
            if (!boid.gameObject.activeInHierarchy) continue;
            if (boid.gameObject.name == this.name) continue;

            float distance = Vector2.Distance(boid.position, this.transform.position);
            if (distance < localRadius)
            {
                otherLocalBoids.Add(boid);
            }
            if (distance < 2 * localRadius)
            {
                otherLocalBoidsDouble.Add(boid);
            }
        }

        //separation
        Vector2 sep = Separation(otherLocalBoids);
        //alignement
        Vector2 ali = Alignement(otherLocalBoids);
        //cohesion
        Vector2 coh = Cohesion(otherLocalBoidsDouble);
        //random direction
        float range = 1;
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);


        velocity += /*joystickInput.magnitude * */randomnessInfluence * new Vector2(x, y).normalized;
        velocity += boidEffect * (sep * separationCoeficient + ali * alignementCoeficient + coh * cohesionCoeficient).normalized;
        velocity += joystickInfluence * joystickInput.normalized;


        velocity.Normalize();
    }

    private Vector2 Separation(List<Transform> otherLocalBoids)
    {
        Vector2 dir = Vector2.zero;
        foreach (Transform localBoid in otherLocalBoids)
        {
            Vector2 other = (Vector2)(localBoid.transform.position - transform.position);
            float dist = Vector2.Distance(localBoid.transform.position, transform.position);
            dir += other / (dist * dist);
        }
        dir /= -otherLocalBoids.Count;
        return dir;
    }
    private Vector2 Alignement(List<Transform> otherLocalBoids)
    {
        Vector2 dir = Vector2.zero;
        foreach (Transform localBoid in otherLocalBoids)
        {
            UnitBOIDS bs = localBoid.GetComponent<UnitBOIDS>();

            dir += bs.velocity;
        }
        dir /= otherLocalBoids.Count;
        return dir;
    }
    private Vector2 Cohesion(List<Transform> otherLocalBoids)
    {
        Vector2 dir = Vector2.zero;
        foreach (Transform localBoid in otherLocalBoids)
        {
            Vector2 other = (Vector2)(localBoid.transform.position - transform.position);
            dir += other;
        }
        dir /= otherLocalBoids.Count;
        return dir;
    }
    #endregion

    void LoopAroundBox()
    {
        if (transform.position.x < -boundaries.x)
        {
            transform.position = new Vector3(boundaries.x - Random.Range(0, 2), transform.position.y);
        }
        if (transform.position.x > boundaries.x)
        {
            transform.position = new Vector3(-boundaries.x + Random.Range(0, 2), transform.position.y);
        }
        if (transform.position.y < -boundaries.y)
        {
            transform.position = new Vector3(transform.position.x, boundaries.y - Random.Range(0, 2));
        }
        if (transform.position.y > boundaries.y)
        {
            transform.position = new Vector3(transform.position.x, -boundaries.y + Random.Range(0, 2));
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

    private void Turn()
    {
        if (velocity.x < 0)
        {
            model.transform.localScale = new Vector3(Mathf.Abs(model.transform.localScale.x), model.transform.localScale.y, model.transform.localScale.z);
            foreach (Transform hand in model.transform)
            {
                hand.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0);
                hand.transform.GetChild(0).transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        if (velocity.x > 0)
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

        horizontal += Input.GetAxis("Horizontal");
        vertical += Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) < 0.01f)
            horizontal = 0;
        if (Mathf.Abs(vertical) < 0.01f)
            vertical = 0;

        //no movement --> regroup
        /*if (horizontal == 0 && vertical == 0)
        {
            Regroup();
        }*/

        CalculateAlteredVelocity(new Vector2(horizontal, vertical));

        transform.Translate(velocity * speed * Time.deltaTime);
    }

    /*private void Regroup()
    {
        //find average position
        Vector2 averagePos = GetAveragePosition();

        //move towards that position
        transform.Translate((averagePos - (Vector2)this.transform.position) * Time.deltaTime);
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
    }*/

    /*public struct DirAndPos
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
    }*/
}
