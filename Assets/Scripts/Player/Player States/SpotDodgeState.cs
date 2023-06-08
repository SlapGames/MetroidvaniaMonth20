using UnityEngine;

public class SpotDodgeState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private VelocityCalculator groundedCalc;

    public SpotDodgeState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;
    }

    public void EnterState()
    {
        animator.Play("Base Layer.Spot Dodge");
        groundedCalc = new VelocityCalculator(0, -1);
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
    }

    public void Run()
    {
        player.VelocityX = groundedCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
    }
}

