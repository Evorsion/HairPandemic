using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    //hp
    public float HP = 100;
    public float maxHP = 100;

    //attack
    public float timeOfAttack = 0;
    public float cooldown = 0.1f;
    public float damage = 1.0f;

    //ui
    public Slider hpBar;


    Fraction fraction;

    private void Start()
    {
        Respawn();

        fraction = this.gameObject.GetComponent<Fraction>();
        if (fraction == null)
        {
            Debug.LogWarning("Living thing should have stats");
        }
    }

    public void DealDamage(float amount, Transform enemyGroup)
    {
        HP -= amount;

        hpBar.value = HP / maxHP;

        if (HP<=0)
        {
            Die(enemyGroup);
        }
    }

    //enviromental damage
    //enviroment can't kill you
    public void DealDamage(float fraction)
    {
        HP -= maxHP * fraction;

        hpBar.value = HP / maxHP;

        if (HP <= 0)
        {
            HP = 1;
        }
    }

    void Die(Transform enemyGroup)
    {
        fraction.Die(enemyGroup);
    }

    public void Respawn()
    {
        HP = maxHP;
        hpBar.value = HP / maxHP;
    }
}
