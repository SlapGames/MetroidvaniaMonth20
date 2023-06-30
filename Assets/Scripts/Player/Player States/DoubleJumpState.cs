using UnityEngine;

public class DoubleJumpState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private float waitBeforeCheckingGroundedStatus = .1f;
    private VelocityCalculator airCalc;
    private float initialSpeed;

    public DoubleJumpState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;
    }

    public void EnterState()
    {
        animator.Play("Base Layer.Jump");

        initialSpeed = player.VelocityX * Mathf.Sign(initialSpeed);

        player.AddJumpForce(player.JumpForce);
        waitBeforeCheckingGroundedStatus += Time.time;

        player.UseDoubleJump();
    }

    public void EvaluateTransitions()
    {
        if (player.CManager.LastHit != null)
        {
            player.ChangeActiveState(nameof(HitStunState));
        }
        else if (waitBeforeCheckingGroundedStatus < Time.time && player.PassiveState == Player.PassiveStates.Grounded)
        {
            player.ChangeActiveState(nameof(NoActionState));
        }
        else if (waitBeforeCheckingGroundedStatus < Time.time && !playerInputManager.JumpHeld)
        {
            player.HaltJump();
            player.ChangeActiveState(nameof(JumpHoverState));
        }
        else if (waitBeforeCheckingGroundedStatus < Time.time && player.Falling)
        {
            //player.ChangeActiveState(nameof(NoActionState));
            player.ChangeActiveState(nameof(JumpHoverState));
        }
    }

    public void ExitState()
    {
    }

    public static string GetName()
    {
        return "DoubleJump";
    }

    public void Run()
    {
        if (playerInputManager.Move != 0)
        {
            airCalc = new VelocityCalculator(playerInputManager.Move * player.DefaultSpeed, player.DefaultAirAcceleration);
        }
        else
        {
            //When there is no movement input, don't accelerat or decelerate, just stay at the same Velocity
            airCalc = new VelocityCalculator(player.VelocityX, player.DefaultAirAcceleration);
        }

        player.VelocityX = airCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
    }
}

