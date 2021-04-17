using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidScript : MonoBehaviour
{
    public Vector2 boundaries;

    public Vector2 velocity;
    public float speed;
    public float localRadius;

    [Range(0, 1)]
    public float separationCoeficient;
    [Range(0, 1)]
    public float alignementCoeficient;
    [Range(0, 1)]
    public float cohesionCoeficient;

    private void Start()
    {
        float xv = Random.Range(-boundaries.x, boundaries.x);
        float yv = Random.Range(-boundaries.y, boundaries.y);
        velocity = new Vector2(xv, yv).normalized;
        this.name = "Boid" + Random.Range(0, 1000000);
        float x = Random.Range(-boundaries.x, boundaries.x);
        float y = Random.Range(-boundaries.y, boundaries.y);
        this.transform.position = new Vector2(x, y);
    }

    private void Update()
    {
        transform.Translate(velocity * Time.deltaTime * speed);
        LoopAroundBox();

        List<Transform> otherLocalBoids = new List<Transform>();
        List<Transform> otherLocalBoidsDouble = new List<Transform>();
        foreach (var boid in this.transform.parent.transform)
        {
            if (((Transform)boid).gameObject.name == this.name)
            {
                continue;
            }
            if (Vector2.Distance(((Transform)boid).position, this.transform.position) < localRadius)
            {
                otherLocalBoids.Add(((Transform)boid));
            }
        }
        foreach (var boid in this.transform.parent.transform)
        {
            if (((Transform)boid).gameObject.name == this.name)
            {
                continue;
            }
            if (Vector2.Distance(((Transform)boid).position, this.transform.position) < 2 * localRadius)
            {
                otherLocalBoidsDouble.Add(((Transform)boid));
            }
        }
        //separation
        Vector2 sep = Separation(otherLocalBoids);
        //alignement
        Vector2 ali = Alignement(otherLocalBoids);
        //cohesion
        Vector2 coh = Cohesion(otherLocalBoidsDouble);

        velocity += 0.1f*(sep * separationCoeficient + ali * alignementCoeficient + coh * cohesionCoeficient).normalized;
        float range = 0.1f;
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);
        velocity += 0.1f * new Vector2(x, y).normalized;
        velocity.Normalize();

    }
    private Vector2 Separation(List<Transform> otherLocalBoids)
    {
        Vector2 dir = Vector2.zero;
        foreach (Transform localBoid in otherLocalBoids)
        {
            Vector2 other = (Vector2)(localBoid.transform.position - transform.position);
            float dist = Vector2.Distance(localBoid.transform.position, transform.position);
            dir += other /(dist*dist);
        }
        dir /= -otherLocalBoids.Count;
        return dir;
    }
    private Vector2 Alignement(List<Transform> otherLocalBoids)
    {
        Vector2 dir = Vector2.zero;
        foreach (Transform localBoid in otherLocalBoids)
        {
            BoidScript bs = localBoid.GetComponent<BoidScript>();
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


    void LoopAroundBox()
    {
        if (transform.position.x < -boundaries.x)
        {
            transform.position = new Vector3(boundaries.x-Random.Range(0,2), transform.position.y);
        }
        if (transform.position.x > boundaries.x)
        {
            transform.position = new Vector3(-boundaries.x + Random.Range(0,2), transform.position.y);
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
}
