using UnityEngine;

public class SkidState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private VelocityCalculator groundedCalc;

    public SkidState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;
    }

    public void EnterState()
    {
        animator.Play("Base Layer.Skid");

        //Move should be 0 at this point, so we use the previous target velocity to determine what direction the skid should be in.
        groundedCalc = new VelocityCalculator(Mathf.Sign(player.VelocityX) * player.DefaultSkidSpeed, player.DefaultGroundedAcceleration);
    }

    public void EvaluateTransitions()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && playerInputManager.Move == 0)
        {
            player.ChangeActiveState(nameof(NoActionState));
        }
        else if(playerInputManager.Move != 0 && playerInputManager.Move != playerInputManager.LastMoveValue)
        {
            player.ChangeActiveState(nameof(PivotState));
        }
        else if(playerInputManager.Move == playerInputManager.LastMoveValue)
        {
            player.ChangeActiveState(nameof(RunState));
        }
        else if (playerInputManager.ReadCurrentInput()?.InputType == InputType.Jump)
        {
            player.ChangeActiveState(nameof(JumpState));
        }
        else if (player.DodgeAvailable && playerInputManager.ReadCurrentInput()?.InputType == InputType.Dodge)
        {
            player.ChangeActiveState(nameof(SpotDodgeState));
        }
    }

    public void ExitState()
    {
    }

    public static string GetName()
    {
        return "Skid";
    }

    public void Run()
    {
        player.VelocityX = groundedCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
    }
}

