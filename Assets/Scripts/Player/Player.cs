using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PassiveStates
    {
        Grounded = 0,
        Airborne = 1,
    }

    public PassiveStates PassiveState { get; private set; } = PassiveStates.Grounded;
    public IPlayerActiveState ActiveState { get; private set; }
    public GrappleManager GManager { get; private set; }
    public CombatManager CManager { get; private set; }
    public bool Falling { get; private set; }
    public bool DoubleJumpAvailable { get; private set; }
    public bool DodgeAvailable { get; private set; }
    public bool BlockAvailable { get; private set; }
    public bool PowerAvailable { get; private set; }
    public bool GrappleAvailable { get; private set; }
    public Vector2 JumpForce
    {
        get { return jumpForce; }
    }
    public Vector2 BackflipForce
    {
        get { return backflipForce; }
    }
    public float DefaultSpeed { get => defaultSpeed; private set => defaultSpeed = value; }
    public float DefaultGroundedAcceleration { get => defaultGroundedAcceleration; private set => defaultGroundedAcceleration = value; }
    public float DefaultAirAcceleration { get => defaultAirAcceleration; private set => defaultAirAcceleration = value; }
    public float DefaultSkidSpeed { get => defaultSkidSpeed; private set => defaultSkidSpeed = value; }
    public float DefaultPivotSpeed { get => defaultPivotSpeed; private set => defaultPivotSpeed = value; }
    public float DefaultPivotAcceleration { get => defaultPivotAcceleration; private set => defaultPivotAcceleration = value; }
    public float DefaultDodgeSpeed { get => defaultDodgeSpeed; private set => defaultDodgeSpeed = value; }
    public float DefaultAirDodgeSpeed { get => defaultAirDodgeSpeed; private set => defaultAirDodgeSpeed = value; }
    public float DefaultGrappleDashSpeed { get => defaultGrappleDashSpeed; private set => defaultGrappleDashSpeed = value; }
    public float DefaultGrappleDashTime { get => defaultGrappleDashTime; private set => defaultGrappleDashTime = value; }
    public float VelocityX { get; set; }
    public float VelocityY { get; set; }
    public float DeltaTimeCopy { get; private set; }
    public float HoverGravity { get => hoverGravity; private set => hoverGravity = value; }
    public float HoverTime { get => hoverTime; private set => hoverTime = value; }
    public float SpriteDirection { get; private set; } = 1;

    public Rigidbody2D rb2d;

    public int groundCheckShouldCheckHowManyColliders = 16;
    public Animator animator;

    [SerializeField] private float minDistanceForFall = .01f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Collider2D groundChecker;
    [SerializeField] private Vector2 jumpForce = new Vector2(0, 7);
    [SerializeField] private Vector2 backflipForce = new Vector2(0, 9);
    [SerializeField] private float defaultSpeed = 3.5f;
    [SerializeField] private float defaultGroundedAcceleration = -1;
    [SerializeField] private float defaultAirAcceleration = 7;
    [SerializeField] private float defaultSkidSpeed = 1.5f;
    [SerializeField] private float defaultPivotSpeed = 2;
    [SerializeField] private float defaultPivotAcceleration = 7;
    [SerializeField] private float defaultDodgeSpeed = 5;
    [SerializeField] private float dodgeCooldown = .75f;
    [SerializeField] private float defaultAirDodgeSpeed = 8;
    [SerializeField] private float defaultGrappleDashSpeed = 8;
    [SerializeField] private float defaultGrappleDashTime = .5f;
    [SerializeField] private float regularGravity = 2;
    [SerializeField] private float hoverGravity = 1;
    [SerializeField] private float hoverTime = .5f;
    [SerializeField] private bool useTargetYVelocity = false;

    [SerializeField] private int energy = 2;
    [SerializeField] private Color[] energyColors = { Color.red, Color.yellow, Color.green };

    private Transform player;
    private Vector2 previousPosition;
    private int maxDoubleJumps = 1;
    private int doubleJumpsUsed = 0;
    private ContactFilter2D groundFilter;
    private PlayerActiveStateFactory activeStateFactory;
    private float dodgeTimer;

    private Material playerMaterial;

    [Space(15)]
    [Header("Debug")]
    public string DEBUG_passiveStateName;
    public string DEBUG_activeStateName;
    public bool DEBUG_falling;
    public float DEBUG_VelocityX;
    public float DEBUG_VelocityY;
    public Vector2 DEBUG_ActualVelocity;
    public bool DEBUG_UseEnergy;

    private void Start()
    {
        activeStateFactory = new PlayerActiveStateFactory(this, GetComponent<PlayerInputManager>(), animator);

        ChangeActiveState(nameof(NoActionState));

        player = GetComponent<Transform>();
        previousPosition = player.position;

        groundFilter.useLayerMask = true;
        groundFilter.layerMask = groundMask;
        groundFilter.useTriggers = true;

        GManager = GetComponent<GrappleManager>();
        CManager = GetComponent<CombatManager>();

        playerMaterial = GetComponent<SpriteRenderer>().material;
        playerMaterial.SetColor("_Color", energyColors[energy]);


        rb2d = GetComponent<Rigidbody2D>();
        DEBUG_ActualVelocity = rb2d.velocity;
        DEBUG_VelocityX = VelocityX;
        DEBUG_VelocityY = VelocityY;
    }

    private void Update()
    {
        DeltaTimeCopy = Time.deltaTime;

        if(energy < 0)
        {
            energy = 0;
            Debug.LogWarning("Energy is less than 0. Setting to 0.");
        }

        //Falling
        //Falling = player.position.y < previousPosition.y && Mathf.Abs(player.position.y - previousPosition.y) >= minDistanceForFall;
        Falling = rb2d.velocity.y < 0;
        DEBUG_falling = Falling;

        if (PassiveState == PassiveStates.Grounded)
        {
            doubleJumpsUsed = 0;
            ResetEnergyLevel();
        }
        DoubleJumpAvailable = doubleJumpsUsed < maxDoubleJumps;

        DodgeAvailable = dodgeTimer < Time.time;

        //ToDo: Limit uses of block. Make it refresh on hit and on succesfully dodging an attack.
        BlockAvailable = true;

        //ToDo: Limit uses of powers. Put it on a cooldown, and don't refresh in the air.
        PowerAvailable = energy > 0;

        GrappleAvailable = energy > 0;
        
        playerMaterial.SetColor("_Color", energyColors[energy]);

        ActiveState.Run();
        ActiveState.EvaluateTransitions();

        previousPosition = player.position;



        DEBUG_passiveStateName = PassiveState.ToString();
        DEBUG_activeStateName = ActiveState.ToString();
        if (DEBUG_UseEnergy)
        {
            energy--;
            DEBUG_UseEnergy = false;
        }
    }

    private void FixedUpdate()
    {
        //Passive State
        if (IsGrounded())
        {
            PassiveState = PassiveStates.Grounded;
        }
        else
        {
            PassiveState = PassiveStates.Airborne;
        }

        float yComponent = rb2d.velocity.y;
        if (useTargetYVelocity)
        {
            yComponent = VelocityY;
        }
        rb2d.velocity = new Vector2(VelocityX, yComponent);
    }

    public void ChangeActiveState(string newState)
    {
        ActiveState?.ExitState();

        ActiveState = activeStateFactory.CreatePlayerActiveState(newState);
        if (ActiveState == null)
        {
            throw new System.Exception("The state factory returned null for state: " + newState);
        }

        //Debug.Log(ActiveState.ToString());
        ActiveState.EnterState();
    }

    public void UseDoubleJump()
    {
        doubleJumpsUsed++;
    }

    public void ResetDoubleJump()
    {
        doubleJumpsUsed = 0;
    }

    private bool IsGrounded()
    {
        int collisionsFound = groundChecker.OverlapCollider(groundFilter, new Collider2D[groundCheckShouldCheckHowManyColliders]);
        return collisionsFound > 0;
    }

    public void SetSpriteDirection(float direction)
    {
        //player.localScale = new Vector2(Mathf.Sign(direction) * Mathf.Abs(player.localScale.x), player.localScale.y);
        GetComponent<SpriteRenderer>().flipX = direction < 0;
        SpriteDirection = direction;
    }

    public void HaltJump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
    }

    public void AddJumpForce(Vector2 force)
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, force.y);
        //rb2d.AddForce(force, ForceMode2D.Impulse);
    }

    public void TriggerDodgeCooldown()
    {
        dodgeTimer = Time.time + dodgeCooldown;
    }

    /// <remarks>
    /// UnHaltYMovement should be called after this at some point, or the player won't be able to move vertically at all.
    /// </remarks>
    public void HaltYMovement()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    /// <remarks>
    /// UnHaltYMovement should be called after this at some point, or the player won't be able to move vertically at all.
    /// </remarks>
    public void HaltPreviousMovement()
    {
        VelocityX = 0;
        HaltYMovement();
    }

    public void UnHaltYMovement()
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void SetGravity(float newGravity)
    {
        rb2d.gravityScale = newGravity;
    }

    public void ResetGravity()
    {
        rb2d.gravityScale = regularGravity;
    }

    public void PrepareForGrapple()
    {

    }

    public void ResetAfterGrapple()
    {

    }

    public void PrepareForGrappleDash()
    {
        useTargetYVelocity = true;
        SetGravity(0);
        VelocityX = 0;
        VelocityY = 0;

        rb2d.velocity = new Vector2(0, 0);
    }
    public void ResetAfterGrappleDash()
    {
        useTargetYVelocity = false;
        ResetGravity();
        VelocityX = 0;
        VelocityY = 0;

        //Velocity needs to be reset here because the y component specifically can cause unintended movement at the end of the dash.
        rb2d.velocity = new Vector2(0, 0);
    }

    public void ResetEnergyLevel()
    {
        energy = energyColors.Length - 1;
    }

    public void UseEnergy()
    {
        energy--;
        if(energy < 0)
        {
            energy = 0;
        }   
    }
}
