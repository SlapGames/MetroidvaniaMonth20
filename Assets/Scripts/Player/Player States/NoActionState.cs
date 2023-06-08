using UnityEngine;

public class NoActionState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private VelocityCalculator groundedCalc;
    private VelocityCalculator airCalc;

    public NoActionState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        groundedCalc = new VelocityCalculator(0, player.DefaultGroundedAcceleration);
        //Velocity in the air shouldn't change.
        airCalc = new VelocityCalculator(player.VelocityX, 0);
    }

    public void EnterState()
    {
        animator.Play("Base Layer.Idle");
    }

    public void EvaluateTransitions()
    {
        if (playerInputManager.Move != 0)
        {
            player.ChangeActiveState(nameof(RunState));
        }
        else if (playerInputManager.ReadCurrentInput()?.InputType == InputType.Jump)
        {
            if (player.PassiveState == Player.PassiveStates.Grounded)
            {
                player.ChangeActiveState(nameof(JumpState));
            }
            else if (player.PassiveState == Player.PassiveStates.Airborne && player.DoubleJumpAvailable)
            {
                player.ChangeActiveState(nameof(DoubleJumpState));
            }
        }
        else if (player.PassiveState == Player.PassiveStates.Grounded && player.DodgeAvailable && playerInputManager.ReadCurrentInput()?.InputType == InputType.Dodge)
        {
            player.ChangeActiveState(nameof(SpotDodgeState));
        }
        else if (player.PassiveState == Player.PassiveStates.Airborne && player.DodgeAvailable && playerInputManager.ReadCurrentInput()?.InputType == InputType.Dodge)
        {
            player.ChangeActiveState(nameof(AirDodgeState));
        }
        else if(playerInputManager.ReadCurrentInput()?.InputType == InputType.Grapple)
        {
            player.ChangeActiveState(nameof(GrappleStartupState));
        }
    }

    public void ExitState()
    {
    }

    public static string GetName()
    {
        return "NoAction";
    }

    public void Run()
    {
        if (player.PassiveState == Player.PassiveStates.Grounded)
        {
            player.VelocityX = groundedCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
        }
        else if (player.PassiveState == Player.PassiveStates.Airborne)
        {
            player.VelocityX = airCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
        }
    }
}

