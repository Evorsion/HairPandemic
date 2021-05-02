using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear_trap : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float damageInPercent = 0.5f * 100;
    [Range(-10.0f, 10.0f)]
    public float verticalOffset = 2.5f;

    private bool used = false;
    private GameObject victim;


    private void Update()
    {
        if (used && victim != null)
        {
            victim.transform.position = this.transform.position + new Vector3(0, victim.transform.localScale.y * verticalOffset, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used)
            return;
        if (IsUnit.isUnit(collision.gameObject))
        {
            used = true;
            GetComponent<Animator>().SetTrigger("Trigger");
            victim = collision.gameObject;
            victim.GetComponent<Stats>().DealDamage(damageInPercent / 100.0f);
        }
    }
}
