using System;
using UnityEngine;

public class OneWayBreakableWall : MonoBehaviour
{
    public enum OpenDirection
    {
        Left, Right,
    }

    [SerializeField] OpenDirection opensFrom = OpenDirection.Left;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player Attack")
        {
            if ((opensFrom == OpenDirection.Left && collision.transform.position.x < transform.position.x) ||
                (opensFrom == OpenDirection.Right && collision.transform.position.x > transform.position.x))
            {
                DestroySelf();
            }
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}