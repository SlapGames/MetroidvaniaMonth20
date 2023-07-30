using UnityEngine;

public class RollyEnemyWallDetector:MonoBehaviour
{
    [SerializeField] private RollyEnemy enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Grapple Point")
        {
            return;
        }

        if(collision.GetComponent<RollyEnemy>() == null || collision.GetComponent<RollyEnemy>() != enemy)
        {
            enemy.Flip();
        }
    }
}