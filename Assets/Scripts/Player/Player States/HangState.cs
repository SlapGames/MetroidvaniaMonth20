using UnityEngine;

public class HangState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    public HangState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;
    }

    public void EnterState()
    {
        player.VelocityX = 0;
        player.HaltYMovement();
    }

    public void EvaluateTransitions()
    {
        if (player.CManager.LastHit != null)
        {
            player.ChangeActiveState(nameof(HitStunState));
        }
        else if (!playerInputManager.GrappleHeld)
        {
            player.ChangeActiveState(nameof(NoActionState));
        }
        else if (playerInputManager.ReadCurrentInput()?.InputType == InputType.Jump)
        {
            player.ChangeActiveState(nameof(JumpState));
        }
    }

    public void ExitState()
    {
        player.UnHaltYMovement();
    }

    public static string GetName()
    {
        return "Hang";
    }

    public void Run()
    {
    }
}

