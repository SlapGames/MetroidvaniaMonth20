using UnityEngine;

public class RunState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private float currentDirection;
    private VelocityCalculator groundedCalc;
    private VelocityCalculator airCalc;

    private bool hasSetAnimation = false;
    public RunState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

    }

    public void EnterState()
    {
        currentDirection = playerInputManager.Move;

        groundedCalc = new VelocityCalculator(Mathf.Sign(playerInputManager.Move) * player.DefaultSpeed, player.DefaultGroundedAcceleration);
        airCalc = new VelocityCalculator(Mathf.Sign(playerInputManager.Move) * player.DefaultSpeed, player.DefaultAirAcceleration);

        player.SetSpriteDirection(Mathf.Sign(currentDirection));
    }

    public void EvaluateTransitions()
    {
        if (player.CManager.LastHit != null)
        {
            player.ChangeActiveState(nameof(HitStunState));
        }
        else if (playerInputManager.Move != currentDirection && player.PassiveState == Player.PassiveStates.Grounded)
        {
            if(playerInputManager.Move == 0)
            {
                player.ChangeActiveState(nameof(SkidState));
            }
            else
            {
                player.ChangeActiveState(nameof(PivotState));
            }
        }
        else if (playerInputManager.Move == 0 && player.PassiveState == Player.PassiveStates.Airborne)
        {
            player.ChangeActiveState(nameof(NoActionState));
        }
        else if (playerInputManager.ReadCurrentInput()?.InputType == InputType.Jump && player.PassiveState == Player.PassiveStates.Grounded)
        {
            player.ChangeActiveState(nameof(JumpState));
        }
        else if (playerInputManager.ReadCurrentInput()?.InputType == InputType.Jump && player.PassiveState == Player.PassiveStates.Airborne && player.DoubleJumpAvailable)
        {
            player.ChangeActiveState(nameof(DoubleJumpState));
        }
        else if(player.DodgeAvailable && player.PassiveState == Player.PassiveStates.Grounded && playerInputManager.ReadCurrentInput()?.InputType == InputType.Dodge)
        {
            player.ChangeActiveState(nameof(DodgeState));
        }
        else if(player.AirDodgeAvailable && player.PassiveState == Player.PassiveStates.Airborne && playerInputManager.ReadCurrentInput()?.InputType == InputType.Dodge)
        {
            player.ChangeActiveState(nameof(AirDodgeState));
        }
        else if(player.GrappleAvailable && playerInputManager.ReadCurrentInput()?.InputType == InputType.Grapple)
        {
            player.ChangeActiveState(nameof(GrappleStartupState));
        }
        else if (player.PassiveState == Player.PassiveStates.Grounded && playerInputManager.ReadCurrentInput()?.InputType == InputType.Attack)
        {
            player.ChangeActiveState(nameof(AttackWindupState));
        }
        else if (player.PowerAvailable && playerInputManager.ReadCurrentInput()?.InputType == InputType.Power)
        {
            player.ChangeActiveState(nameof(PsychicPowerWindupState));
        }
        else if (player.BlockAvailable && playerInputManager.ReadCurrentInput()?.InputType == InputType.Block)
        {
            player.ChangeActiveState(nameof(BlockState));
        }
    }

    public void ExitState()
    {
        playerInputManager.LastMoveValue = currentDirection;
    }

    public static string GetName()
    {
        return "Run";
    }

    public void Run()
    {
        if (player.PassiveState == Player.PassiveStates.Grounded)
        {
            if (!hasSetAnimation)
            {
                animator.Play("Base Layer.Run");
                hasSetAnimation = true;
            }
            player.VelocityX = groundedCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
        }
        else if (player.PassiveState == Player.PassiveStates.Airborne)
        {
            player.VelocityX = airCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
        }
    }
}

