using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePointDetector : MonoBehaviour
{
    public GrapplePoint PointToGrappleTo { get; private set; }

    [SerializeField] private GrapplePoint defaultPoint;
    [SerializeField] private Transform furthestPoint;
    [SerializeField] private LayerMask grappleStoppedBy_Mask;
    [SerializeField] private LayerMask grapplePointOnlyMask;
    [SerializeField] private Transform measureFrom;

    private Rigidbody2D body;
    private ContactFilter2D grapplePointContactFilter;
    private const int COLLIDERS_TO_CHECK = 8;
    private bool pause = false;

    [Space(15)]
    [Header("Debug")]
    GrapplePoint DEBUG_ToGrappleTo;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        grapplePointContactFilter.layerMask = grapplePointOnlyMask;
        grapplePointContactFilter.useLayerMask = true;
        grapplePointContactFilter.useTriggers = true;
    }

    private void Update()
    {
        DEBUG_ToGrappleTo = PointToGrappleTo;
    }

    private void FixedUpdate()
    {
        if (!pause)
        {
            DetectPoints();
        }
    }

    public void Pause()
    {
        pause = true;
    }
    public void UnPause()
    {
        pause = false;
    }

    public void DetectPoints()
    {
        if(PointToGrappleTo != null)
        {
            PointToGrappleTo.Active = false;
        }

        Collider2D[] results = new Collider2D[COLLIDERS_TO_CHECK];
        int numberOfResults = body.OverlapCollider(grapplePointContactFilter, results);

        GrapplePoint current = null;

        for (int i = 0; i < numberOfResults; i++)
        {
            GrapplePoint potentialNewPoint = results[i].GetComponent<GrapplePoint>();

            if (potentialNewPoint == null)
            {
                continue;
            }

            if (!potentialNewPoint.Valid)
            {
                continue;
            }

            if (Physics2D.Linecast(measureFrom.position, potentialNewPoint.transform.position, grappleStoppedBy_Mask).collider == null)
            {
                if (current == null ||
                        Vector2.Distance(measureFrom.position, potentialNewPoint.transform.position) < Vector2.Distance(measureFrom.position, current.transform.position))
                {
                    current = potentialNewPoint;
                }
            }
        }

        if(current == null)
        {
            current = defaultPoint;
            current.transform.position = furthestPoint.position;
            
            Collider2D collision = Physics2D.Linecast(measureFrom.position, furthestPoint.position, grappleStoppedBy_Mask).collider;
            if (collision != null) 
            {
                current.transform.position = collision.transform.position;
            }
        }

        PointToGrappleTo = current;
        PointToGrappleTo.Active = true;
    }
}
