using UnityEngine;

public class RollyEnemyTelekinesis1Handler: Telekinesis1Susceptible 
{
    private Telekinesis1Manager manager;
    private RollyEnemy self;
    private Vector2 targetPoint;

    private bool lift;
    private bool grapplePointActivated;

    private void Start()
    {
        self = GetComponent<RollyEnemy>();
    }

    public override void HandleAffectedByTele1Windup(Telekinesis1Manager telekinesis1Manager)
    {
        manager = telekinesis1Manager;
        self.PrepareForLift();

        targetPoint = transform.position + manager.LiftHeight;
    }
    public override void HandleAffectedByTele1Action()
    {
        self.Lift();
        lift = true;
    }
    public override void HandleAffectedByTele1Deactivate()
    {
        self.Drop();
        self.DeactivateGrapplePoint();

        lift = false;
        grapplePointActivated = false;
    }

    private void Update()
    {
        if(lift)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, manager.LiftSpeed * Time.deltaTime);
            if(!grapplePointActivated && Vector3.Distance(transform.position, targetPoint) <= .1f)
            {
                grapplePointActivated= true;
                self.ActivateGrapplePoint();
            }
        }
    }

}
