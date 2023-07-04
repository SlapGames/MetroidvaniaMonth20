using UnityEngine;

public class GrappleMovementState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private GrappleManager grappleManager;

    public GrappleMovementState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        grappleManager = player.GManager;
    }

    public void EnterState()
    {
        animator.Play("Base Layer.Grapple Drag Horizontal");

        player.HaltYMovement();

        player.StartGrappleAfterImage();

        grappleManager.StartPlayerMovement();
    }

    public void EvaluateTransitions()
    {
        if (!grappleManager.PlayerMoving && playerInputManager.AttackHeld)
        {
            player.ChangeActiveState(nameof(GrappleDashState));
        }
        else if (!grappleManager.PlayerMoving && !playerInputManager.GrappleHeld)
        {
            player.ChangeActiveState(nameof(NoActionState));
        }
        else if (!grappleManager.PlayerMoving && playerInputManager.GrappleHeld)
        {
            player.ChangeActiveState(nameof(HangState));
        }
    }

    public void ExitState()
    {
        player.StopGrappleAfterImage();

        player.UnHaltYMovement();
        grappleManager.ResetGrappling();
    }

    public static string GetName()
    {
        return "GrappleMovement";
    }

    public void Run()
    {
    }
}

