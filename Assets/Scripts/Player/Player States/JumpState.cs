using UnityEngine;

public class JumpState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private float waitBeforeCheckingGroundedStatus = .1f;
    private VelocityCalculator airCalc;
    private float initialSpeed;

    public JumpState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;
    }

    public void EnterState()
    {
        if(player.CurrentJumpType == Player.JumpType.Regular)
        {
            player.AddJumpForce(player.JumpForce);
        }
        else if(player.CurrentJumpType == Player.JumpType.Long)
        {
            player.AddJumpForceIncludeX(player.LongJumpForce);
        }

        animator.Play("Base Layer.Jump");

        //Initial speed equals the player's Velocity.x, but always positive.
        initialSpeed = player.VelocityX * Mathf.Sign(initialSpeed);

        waitBeforeCheckingGroundedStatus += Time.time;
    }

    public void EvaluateTransitions()
    {
        if(player.CManager.LastHit != null)
        {
            player.ChangeActiveState(nameof(HitStunState));
        }
        else if(waitBeforeCheckingGroundedStatus < Time.time && player.PassiveState == Player.PassiveStates.Grounded)
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
            player.ChangeActiveState(nameof(JumpHoverState));
        }
    }

    public void ExitState()
    {
        player.CurrentJumpType = Player.JumpType.Regular;
    }

    public static string GetName()
    {
        return "Jump";
    }

    public void Run()
    {

        if(playerInputManager.Move != 0)
        {
            airCalc = new VelocityCalculator(playerInputManager.Move * player.DefaultSpeed, player.DefaultAirAcceleration);
        }
        else
        {
            //When there is no movement input, don't accelerate or decelerate, just stay at the same Velocity
            airCalc = new VelocityCalculator(player.VelocityX, player.DefaultAirAcceleration);
        }

        player.VelocityX = airCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
    }
}
