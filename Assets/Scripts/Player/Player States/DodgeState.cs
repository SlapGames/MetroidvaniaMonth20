using UnityEngine;

public class DodgeState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private VelocityCalculator groundedCalc;

    public DodgeState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;
    }

    public void EnterState()
    {
        animator.Play("Base Layer.Roll");
        groundedCalc = new VelocityCalculator(Mathf.Sign(playerInputManager.Move) * player.DefaultDodgeSpeed, player.DefaultGroundedAcceleration);
    }

    public void EvaluateTransitions()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            player.ChangeActiveState(nameof(NoActionState));
        }
    }

    public void ExitState()
    {
        player.TriggerDodgeCooldown();
        player.CManager.DisregardLastHit();
    }

    public static string GetName()
    {
        return "Dodge";
    }

    public void Run()
    {
        player.VelocityX = groundedCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
    }
}

