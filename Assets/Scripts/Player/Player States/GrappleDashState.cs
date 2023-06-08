using UnityEngine;

public class GrappleDashState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private GrappleManager grappleManager;
    private VelocityCalculator xCalc;
    private VelocityCalculator yCalc;

    private float timer;

    public GrappleDashState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        grappleManager = player.GManager;
    }

    public void EnterState()
    {
        animator.Play("Base Layer.Grapple Dash");
        player.PrepareForGrappleDash();

        timer = Time.time + player.DefaultGrappleDashTime;

        xCalc = new VelocityCalculator(grappleManager.Direction.x * player.DefaultGrappleDashSpeed, -1);
        yCalc = new VelocityCalculator(grappleManager.Direction.y * player.DefaultGrappleDashSpeed, -1);
    }

    public void EvaluateTransitions()
    {
        if (timer < Time.time)
        {
            player.ChangeActiveState(nameof(NoActionState));
        }
    }

    public void ExitState()
    {
        player.ResetAfterGrappleDash();
    }

    public static string GetName()
    {
        return "GrappleDash";
    }

    public void Run()
    {
        player.VelocityX = xCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
        player.VelocityY = yCalc.CalculateNextVelocity(player.VelocityY, player.DeltaTimeCopy);
    }
}

