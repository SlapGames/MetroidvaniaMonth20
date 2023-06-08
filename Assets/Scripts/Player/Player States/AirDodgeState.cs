using UnityEngine;

public class AirDodgeState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private VelocityCalculator airCalc;

    public AirDodgeState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;
    }

    public void EnterState()
    {
        animator.Play("Base Layer.Air Dodge");

        float direction = Mathf.Sign(playerInputManager.Move);
        if (playerInputManager.Move == 0)
        {
            direction = Mathf.Sign(playerInputManager.LastMoveValue);
        }

        airCalc = new VelocityCalculator(direction * player.DefaultAirDodgeSpeed, -1);

        player.HaltYMovement();
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
        player.UnHaltYMovement();
        player.VelocityX = 0;

        player.TriggerDodgeCooldown();
    }

    public static string GetName()
    {
        return "AirDodge";
    }

    public void Run()
    {
        player.VelocityX = airCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
    }
}

