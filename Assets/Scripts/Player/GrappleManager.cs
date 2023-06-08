using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleManager : MonoBehaviour
{
    public bool HookMoving { get; private set; } = false;
    public bool LatchedOn { get; private set; } = false;
    public bool PlayerMoving { get; private set; } = false;
    public Vector2 Direction { get; private set; }

    [SerializeField] private GameObject grappleHook;
    [SerializeField] private GameObject grapplePlayer;
    [SerializeField] private Transform grappleHookOrigin;
    [SerializeField] private float maxDistance;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask grapplePointMask;
    [SerializeField] private GrapplePointDetector grapplePointDetector;

    private GrapplePoint destination;
    private List<GrapplePoint> validGrapplePoints = new List<GrapplePoint>();

    [Space(15)]
    [Header("Debug")]
    public Vector2 DEBUG_direction;

    public void StartGrapple()
    {
        grapplePointDetector.DetectPoints();
        grapplePointDetector.Pause();

        destination = grapplePointDetector.PointToGrappleTo;

        grappleHook.transform.position = grappleHookOrigin.position;

        Direction = new Vector2(destination.transform.position.x - grappleHook.transform.position.x, destination.transform.position.y - grappleHook.transform.position.y);
        Direction = Direction.normalized;
        DEBUG_direction = Direction;

        HookMoving = true;
    }

    public void StartPlayerMovement()
    {
        PlayerMoving = true;
    }

    public void ResetGrappling()
    {
        PlayerMoving = false;
        HookMoving = false;
        LatchedOn = false;
        grapplePointDetector.UnPause();
    }

    private void Update()
    {
        if (HookMoving)
        {
            HandleHookMovement();
        }
        else if(PlayerMoving)
        {
            HandlePlayerMovement();
        }

    }

    private void FixedUpdate()
    {
        foreach(GrapplePoint point in validGrapplePoints)
        {
            point.Valid = false;
        }

        validGrapplePoints.Clear();

        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, maxDistance, grapplePointMask);

        foreach(Collider2D r in results)
        {
            GrapplePoint newGrapplePoint = r.GetComponent<GrapplePoint>();
            if(newGrapplePoint == null)
            {
                Debug.LogWarning("GrapplePointMask detected an object that wasn't actually a grapple point: ", newGrapplePoint.gameObject);
                continue;
            }

            newGrapplePoint.Valid = true;
            validGrapplePoints.Add(newGrapplePoint);
        }
    }

    private void HandleHookMovement()
    {
        grappleHook.transform.position = Vector2.MoveTowards(grappleHook.transform.position, destination.transform.position, speed * Time.deltaTime);

        if((Vector2)grappleHook.transform.position == (Vector2)destination.transform.position)
        {
            HookMoving = false;
            LatchedOn = !destination.Default;
        }
    }
    private void HandlePlayerMovement()
    {
        grapplePlayer.transform.position = Vector2.MoveTowards(grapplePlayer.transform.position, grappleHook.transform.position, speed * Time.deltaTime);

        if((Vector2)grapplePlayer.transform.position == (Vector2)grappleHook.transform.position)
        {
            PlayerMoving = false;
            LatchedOn = false;
        }
    }

    
}
