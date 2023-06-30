using UnityEngine;

class JumpHoverState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private float hoverTimer;

    public JumpHoverState(Player playerToProvide, PlayerInputManager playerInputManagerToProvide, Animator animatorToProvide)
    {
        player = playerToProvide;
        playerInputManager = playerInputManagerToProvide;
        animator = animatorToProvide;
    }

    public void EnterState()
    {
        animator.Play("Base Layer.Jump");

        hoverTimer = Time.time + player.HoverTime;
        player.SetGravity(player.HoverGravity);
        player.HaltJump();
    }

    public void EvaluateTransitions()
    {
        if (player.CManager.LastHit != null)
        {
            player.ChangeActiveState(nameof(HitStunState));
        }
        else if (hoverTimer < Time.time /*|| !playerInputManager.JumpHeld*/)
        {
            player.ChangeActiveState(nameof(NoActionState));
        }
        else if (player.DoubleJumpAvailable && playerInputManager.ReadCurrentInput()?.InputType == InputType.Jump)
        {
            player.ChangeActiveState(nameof(DoubleJumpState));
        }
        else if (player.DodgeAvailable && playerInputManager.ReadCurrentInput()?.InputType == InputType.Dodge)
        {
            player.ChangeActiveState(nameof(AirDodgeState));
        }
        else if (player.GrappleAvailable && playerInputManager.ReadCurrentInput()?.InputType == InputType.Grapple)
        {
            player.ChangeActiveState(nameof(GrappleStartupState));
        }
    }

    public void ExitState()
    {
        player.ResetGravity();
    }

    public void Run()
    {
    }
}
