using UnityEngine;

public class BackflipState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private float waitBeforeCheckingGroundedStatus = .2f;
    private VelocityCalculator airCalc;

    public BackflipState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;
    }

    public void EnterState()
    {
        animator.Play("Base Layer.Backflip");

        player.rb2d.AddForce(player.BackflipForce, ForceMode2D.Impulse);
        waitBeforeCheckingGroundedStatus += Time.time;

        airCalc = new VelocityCalculator(0, -1);
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
        else if (player.Falling)
        {
            player.ChangeActiveState(nameof(JumpHoverState));
        }
    }

    public void ExitState()
    {
    }

    public static string GetName()
    {
        return "Backflip";
    }

    public void Run()
    {
        player.VelocityX = airCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
    }
}

