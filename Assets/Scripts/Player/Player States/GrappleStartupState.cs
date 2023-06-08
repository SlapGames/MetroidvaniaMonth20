using UnityEngine;

public class GrappleStartupState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private GrappleManager grappleManager;

    public GrappleStartupState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        grappleManager = player.GManager;
    }

    public void EnterState()
    {
        player.VelocityX = 0;
        player.HaltYMovement();

        grappleManager.StartGrapple();

        player.SetSpriteDirection(Mathf.Sign(grappleManager.Direction.x));
    }

    public void EvaluateTransitions()
    {
        if(!grappleManager.HookMoving && !grappleManager.LatchedOn)
        {
            grappleManager.ResetGrappling();
            player.ChangeActiveState(nameof(NoActionState));
        }
        else if (!grappleManager.HookMoving && grappleManager.LatchedOn)
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
    }
}

