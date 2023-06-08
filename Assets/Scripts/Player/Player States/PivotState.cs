using UnityEngine;

public class PivotState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private float currentDirection;
    private VelocityCalculator groundedCalc;

    public PivotState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;
    }

    public void EnterState()
    {
        animator.Play("Base Layer.Pivot");
        currentDirection = playerInputManager.Move;

        groundedCalc = new VelocityCalculator(currentDirection * player.DefaultPivotSpeed, player.DefaultGroundedAcceleration);
    }

    public void EvaluateTransitions()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .9)
        {
            player.ChangeActiveState(nameof(NoActionState));
        }
        else if(playerInputManager.ReadCurrentInput()?.InputType == InputType.Jump)
        {
            player.ChangeActiveState(nameof(BackflipState));
        }
        else if (player.DodgeAvailable && playerInputManager.ReadCurrentInput()?.InputType == InputType.Dodge)
        {
            player.ChangeActiveState(nameof(DodgeState));
        }
    }

    public void ExitState()
    {
        player.SetSpriteDirection(Mathf.Sign(currentDirection));
    }

    public static string GetName()
    {
        return "Pivot";
    }

    public void Run()
    {
        player.VelocityX = groundedCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
    }
}

