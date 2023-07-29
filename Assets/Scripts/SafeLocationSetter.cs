using UnityEngine;

public class SafeLocationSetter : MonoBehaviour
{
    [SerializeField] private Transform lastSafePosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Player>().LastSafePosition = lastSafePosition.position;
        }
    }
}

