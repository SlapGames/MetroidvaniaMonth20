using System;
using Unity.VisualScripting;
using UnityEngine;

public class RollyEnemy:MonoBehaviour
{
    [SerializeField] private GrapplePoint grapplePoint;
    [SerializeField] private float direction = 1;
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private RollyEnemyWallDetector wallDetector;

    private Rigidbody2D body;
    private Animator animator;
    private bool dropping;


    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        DeactivateGrapplePoint();
    }

    public void PrepareForLift()
    {
        HaltMovement();
        HaltGravity();

        animator.Play($"Base Layer.Grab");
    }

    private void HaltGravity()
    {
        body.gravityScale = 0;
    }

    private void HaltMovement()
    {
        body.velocity = Vector3.zero;
        direction = 0;
    }

    public void Lift()
    {
        animator.Play($"Base Layer.Held Up");
    }

    public void Drop()
    {
        animator.Play($"Base Layer.Drop");
        dropping = true;

        ResumeGravity();
        ResumeMovement();
    }

    private void ResumeGravity()
    {
        body.gravityScale = 2;
    }

    private void ResumeMovement()
    {
        direction = 1;
        GetComponent<SpriteRenderer>().flipX = direction < 0 ? true : false;
        wallDetector.transform.localPosition = new Vector3(Mathf.Sign(wallDetector.transform.localPosition.x) * wallDetector.transform.localPosition.x * direction, wallDetector.transform.localPosition.y, wallDetector.transform.localPosition.z);
    }

    public void ActivateGrapplePoint()
    {
        grapplePoint.gameObject.SetActive(true);
        GetComponent<Collider2D>().enabled = false;
    }
    public void DeactivateGrapplePoint()
    {
        grapplePoint.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = true;
    }

    public void Flip()
    {
        direction *= -1;
        GetComponent<SpriteRenderer>().flipX = direction < 0 ? true : false;
        wallDetector.transform.localPosition = new Vector3(Mathf.Sign(wallDetector.transform.localPosition.x) * wallDetector.transform.localPosition.x * direction, wallDetector.transform.localPosition.y, wallDetector.transform.localPosition.z);
    }

    private void Update()
    {
        if(dropping && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            dropping = false;

            animator.Play($"Base Layer.Move");
        }
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(direction * speed, body.velocity.y);
    }
}
public class AbilityPickup 
{ 
    
}
