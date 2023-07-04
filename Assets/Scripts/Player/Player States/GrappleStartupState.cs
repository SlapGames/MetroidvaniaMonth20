using UnityEditor.Networking.PlayerConnection;
using UnityEngine;

public class GrappleStartupState : IPlayerActiveState
{
    private const float GRAPPLE_WIND_UP_TIME = .5f;

    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private GrappleManager grappleManager;
    private bool grappleStarted;

    public GrappleStartupState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        grappleManager = player.GManager;
    }

    public void EnterState()
    {
        animator.Play("Base Layer.Grapple Wind Up Horizontal");

        player.HaltPreviousMovement();

        player.UseEnergy();

        grappleManager.SetUpGrapple();

        player.SetSpriteDirection(Mathf.Sign(grappleManager.Direction.x));
    }

    public void EvaluateTransitions()
    {
        if (player.CManager.LastHit != null)
        {
            grappleManager.ResetGrappling();
            player.ChangeActiveState(nameof(HitStunState));
        }
        else if (grappleStarted && !grappleManager.HookMoving && !grappleManager.LatchedOn)
        {
            grappleManager.ResetGrappling();
            player.ChangeActiveState(nameof(NoActionState));
        }
        else if (grappleStarted &&  !grappleManager.HookMoving && grappleManager.LatchedOn)
        {
            player.ChangeActiveState(nameof(GrappleMovementState));
        }
    }

    public void ExitState()
    {
        player.UnHaltYMovement();
    }

    public static string GetName()
    {
        return "GrappleStartup";
    }

    public void Run()
    {
        if(!grappleStarted && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= GRAPPLE_WIND_UP_TIME)
        {
            grappleManager.StartGrapple();
            grappleStarted = true;
        }
    }
}

