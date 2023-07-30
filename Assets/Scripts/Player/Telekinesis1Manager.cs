using UnityEngine;

public class Telekinesis1Manager:MonoBehaviour
{
    public Vector3 LiftHeight { get => liftHeight; private set => liftHeight = value; }
    public float LiftSpeed { get => liftSpeed; private set => liftSpeed = value; }
    public bool WindupWasSuccessful { get => target != null; }

    [SerializeField] private Collider2D hitbox;
    [SerializeField] private Transform hitboxPivot;
    [SerializeField] SpriteRenderer playerRenderer;
    [SerializeField] LayerMask telekinesis1Mask;
    private Telekinesis1Susceptible target;

    [Space(15)]
    [Header("Lifting")]
    [SerializeField] private Vector3 liftHeight;
    [SerializeField] private float liftSpeed;

    private ContactFilter2D contactFilter;


    private void Start()
    {
        contactFilter.layerMask = telekinesis1Mask;
        contactFilter.useLayerMask= true;
        contactFilter.useTriggers= true;
    }

    public void HandleWindup()
    {
        Collider2D[] results = new Collider2D[16];
        int numberOfResults = hitbox.OverlapCollider(contactFilter, results);

        for (int i = 0; i < numberOfResults; i++)
        {
            Telekinesis1Susceptible current = results[i].GetComponent<Telekinesis1Susceptible>();
            if (current != null)
            {
                target = current;
                current.HandleAffectedByTele1Windup(this);
                break;
            }
        }
    }
    public void HandleAction()
    {
        playerRenderer.GetComponent<Player>().EnergyPenalties.Add(1);
        target.HandleAffectedByTele1Action();
    }
    public void HandleDeactivate()
    {
        if(target != null)
        {
            playerRenderer.GetComponent<Player>().EnergyPenalties.Remove(1);
            target.HandleAffectedByTele1Deactivate();
            target = null;
        }
    }

    private void Update()
    {
        HandleHitboxFlipping();
    }

    private void HandleHitboxFlipping()
    {
        Vector3 currentScale = hitboxPivot.localScale;
        hitboxPivot.localScale = new Vector3(playerRenderer.flipX ? -1 : 1, currentScale.y, currentScale.z);
    }
}
