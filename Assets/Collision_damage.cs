using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_damage : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float damageInPercent = 0.5f * 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsUnit.isUnit(collision.gameObject))
        {
            HitWithVehicle(collision.gameObject);
        }
    }

    private void HitWithVehicle(GameObject victim)
    {
        victim.GetComponent<Stats>().DealDamage(damageInPercent / 100.0f);
    }
}
