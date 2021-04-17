using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fraction : MonoBehaviour
{
    public enum Team { Player, Enemy, None };
    public Team team;

    Unit_list listOfUnits;

    GameObject changeFractionEffect;

    List<GameObject> peopleCloseToInteract = new List<GameObject>();

    Stats stats;

    public GameObject myWeapon;
    bool attacking = false;
    List<GameObject> problemPersons = new List<GameObject>();

    public AudioSource weaponSFX;

    private void Start()
    {
        stats = this.gameObject.GetComponent<Stats>();
        if (stats == null) Debug.LogWarning("Skript Stats není správně nastaven");
        listOfUnits = GameObject.Find("#UnitAuthority").GetComponent<Unit_list>();
        if (listOfUnits == null) Debug.LogWarning("Skript Unit_list není správně nastaven");
        changeFractionEffect = listOfUnits.GetChangeFractionEffect();
        if (changeFractionEffect == null) Debug.LogWarning("ChangeFractionEffect není správně nastaven");
    }

    private void Update()
    {
        WeaponAnimation();

        DetermineInteractions();
    }

    private void WeaponAnimation()
    {
        if (myWeapon != null)
        {
            myWeapon.SetActive(attacking);
            if (stats.damage <= 0)
            {
                myWeapon.SetActive(false);
            }
        }
    }

    #region MaintainPeopleCloseToInteractVariable
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fraction otherFraction = collision.gameObject.GetComponent<Fraction>();

        if (otherFraction != null && otherFraction.team != Team.None)
        {
            peopleCloseToInteract.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Fraction otherFraction = collision.gameObject.GetComponent<Fraction>();

        if (otherFraction != null)
        {
            if (peopleCloseToInteract.Contains(collision.gameObject))
            {
                peopleCloseToInteract.Remove(collision.gameObject);
            }
        }
    }
    #endregion

    private void DetermineInteractions()
    {
        bool canAttackAtLeastOne = false;
        if (peopleCloseToInteract.Count > 0)
        {
            problemPersons.Clear();
            foreach (GameObject person in peopleCloseToInteract)
            {
                //detecting non existent members of "peopleCloseToInteract" list
                if (person == null || person.Equals(null))
                {
                    problemPersons.Add(person);
                    continue;
                }
                //if "person" is enemy --> attack
                if (person.GetComponent<Fraction>().team != this.team && person.GetComponent<Fraction>().team != Team.None)
                {
                    Attack(person.GetComponent<Fraction>().GetComponent<Stats>(), this.transform.parent);
                    canAttackAtLeastOne = true;
                    break;
                }
            }
            //fix for destroing person in someones list
            foreach (GameObject problem in problemPersons)
            {
                peopleCloseToInteract.Remove(problem);
            }
            problemPersons.Clear();
        }
        attacking = canAttackAtLeastOne;
    }

    

    void Attack(Stats enemyScript, Transform myGroup /*for conversion*/)
    {
        attacking = true;
        if (enemyScript == null) Debug.LogWarning("Attacking non living thing");

        //point spray
        PointSpray(enemyScript);

        if (stats.timeOfAttack < Time.time)
        {
            //affect stats
            stats.timeOfAttack = Time.time + stats.cooldown;
            enemyScript.DealDamage(stats.damage, myGroup);
        }
    }

    private void PointSpray(Stats enemyScript)
    {
        if (myWeapon != null)
        {
            Vector2 relativePos = (enemyScript.transform.position - this.transform.position).normalized;
            float rot_z = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
            myWeapon.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }
    }

    public void Die(Transform myNewGroup)
    {
        switch (team)
        {
            case Team.Player:
                DiePermanently();
                break;
            case Team.Enemy:
                JoinPlayer(myNewGroup);
                break;
            default:
                break;
        }
    }

    void JoinPlayer(Transform enemyGroup)
    {
        //changeFraction effect
        GameObject effect = Instantiate(changeFractionEffect, this.transform.position, Quaternion.identity) as GameObject;
        effect.GetComponent<ParticleSystem>().Play();

        //spawn new unit of same lvl in enemyGroup
        SpawnConvertedPlayerUnit(enemyGroup);

        //destroy me
        Destroy(this.gameObject);

        /*stats.Respawn();
        GetComponentInChildren<SpriteRenderer>().sprite = playerTeamSprite;

        //switch visual weapon
        myWeapon.SetActive(false);
        myWeapon = myAltWeapon;

        //switch group for counting purposes
        this.transform.parent = GameObject.Find("#PlayerUnits").transform;*/
    }

    private void SpawnConvertedPlayerUnit(Transform enemyGroup)
    {
        int myLevel = int.Parse(this.name.Substring(this.name.IndexOf('.') + 1))-1;
        GameObject newUnit = Instantiate(listOfUnits.GetPlayerUnit(myLevel).prefab, this.transform.position, Quaternion.identity, enemyGroup) as GameObject;
        newUnit.name = listOfUnits.GetPlayerUnit(myLevel).name;
    }

    private void SpawnConvertedDeadUnit()
    {
        int myLevel = int.Parse(this.name.Substring(this.name.IndexOf('.') + 1))-1;
        GameObject newUnit = Instantiate(listOfUnits.GetDeadUnit(myLevel).prefab, this.transform.position, Quaternion.identity) as GameObject;
        newUnit.name = listOfUnits.GetDeadUnit(myLevel).name;
    }

    void DiePermanently()
    {
        //changeFraction effect
        GameObject effect = Instantiate(changeFractionEffect, this.transform.position,Quaternion.identity)as GameObject;
        effect.GetComponent<ParticleSystem>().Play();

        //spawn new unit of same lvl in enemyGroup
        SpawnConvertedDeadUnit();

        //destroy me
        Destroy(this.gameObject);


        /*//change sprite to hairless
        this.GetComponentInChildren<SpriteRenderer>().sprite = noneTeamSprite;
        //disassociate with leader
        this.GetComponent<FollowLeader>().SetLeader(null);
        //turn team to none
        team = Team.None;
        //turn off attack
        stats.damage = 0;

        //switch group for counting purposes
        this.transform.parent = GameObject.Find("#PermaDeath").transform;

        //Destroy(gameObject);*/
    }
}
