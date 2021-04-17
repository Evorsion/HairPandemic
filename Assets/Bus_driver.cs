using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus_driver : MonoBehaviour
{
    public Direction direction = Direction.Right;
    public float speed;
    public Sprite[] busSprites;


    private Vector2 directionVector;

    private void Start()
    {
        directionVector = ConvertDirectionToVector(this.GetComponent<SpriteRenderer>());
    }

    void Update()
    {
        transform.Translate(directionVector * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Turn_marker marker = collision.transform.parent.GetComponent<Turn_marker>();
        if (marker != null)
        {
            direction = marker.TurnDirection;
            directionVector = ConvertDirectionToVector(this.GetComponent<SpriteRenderer>());
        }
    }

    private Vector2 ConvertDirectionToVector(SpriteRenderer sr)
    {
        Vector2 directionVector = Vector2.zero;
        switch (direction)
        {
            case Direction.Up:
                directionVector = Vector2.up;
                sr.sprite = busSprites[0];
                break;
            case Direction.Down:
                directionVector = Vector2.down;
                sr.sprite = busSprites[2];
                break;
            case Direction.Left:
                directionVector = Vector2.left;
                sr.sprite = busSprites[3];
                break;
            case Direction.Right:
                directionVector = Vector2.right;
                sr.sprite = busSprites[1];
                break;
            default:
                Debug.LogError("Direction není v seznamu");
                break;
        }
        return directionVector;
    }
}
