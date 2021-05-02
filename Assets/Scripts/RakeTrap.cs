using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RakeTrap : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float damageInPercent = 0.5f *100;
    public Sprite onGround;
    public Sprite inAir;
    enum RakeState { OnGround, InAir , Reseting}
    RakeState state = RakeState.OnGround;

    public float resetTime = 1f;
    private float timeOfReset = -1;

    List<GameObject> unitsInProximityDistance = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsUnit.isUnit(collision.gameObject))
        {
            if (!unitsInProximityDistance.Contains(collision.gameObject))
            {
                unitsInProximityDistance.Add(collision.gameObject);
            }
        }
        UpdateRakeState();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsUnit.isUnit(collision.gameObject))
        {
            if (unitsInProximityDistance.Contains(collision.gameObject))
            {
                unitsInProximityDistance.Remove(collision.gameObject);
            }
        }
        UpdateRakeState();
    }

    private void UpdateRakeState()
    {
        //functionality
        switch (state)
        {
            case RakeState.OnGround:
                //hitted someone
                if (unitsInProximityDistance.Count != 1) Debug.LogWarning("Počet jednotek v přítomnosti letících hrábí má být 1");
                if (unitsInProximityDistance.Count >= 1) HitWithRake(unitsInProximityDistance[0]);
                state = RakeState.InAir;
                break;
            case RakeState.InAir:
                if (unitsInProximityDistance.Count <= 0){
                    state = RakeState.Reseting;
                    timeOfReset = Time.time + resetTime;
                }
                break;
            case RakeState.Reseting:
                if (timeOfReset <= Time.time)
                {
                    state = RakeState.OnGround;
                }
                break;
            default:
                break;
        }

        //graphics
        switch (state)
        {
            case RakeState.OnGround:
                this.GetComponent<SpriteRenderer>().sprite = onGround;
                break;
            case RakeState.InAir:
                this.GetComponent<SpriteRenderer>().sprite = inAir;
                break;
            case RakeState.Reseting:
                this.GetComponent<SpriteRenderer>().sprite = onGround;
                break;
            default:
                break;
        }
    }

    private void HitWithRake(GameObject victim)
    {
        victim.GetComponent<Stats>().DealDamage(damageInPercent / 100.0f);
    }

}
