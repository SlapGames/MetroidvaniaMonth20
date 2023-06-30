using UnityEngine;

public class BlockStunState: IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private CombatManager combatManager;

    private VelocityCalculator knockBackCalc;
    private float knockBackWindow = .4f;
    private float blockStunPercentage = .5f;

    public BlockStunState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        combatManager = player.CManager;
    }

    public void EnterState()
    {
        player.HaltPreviousMovement();

        player.SetSpriteDirection(-combatManager.LastHit.Direction);

        animator.Play($"Base Layer.Block Stun");
        knockBackCalc = new VelocityCalculator(blockStunPercentage * combatManager.LastHit.Direction * combatManager.LastHit.KnockBack, -1);

        combatManager.ProcessLastHit();
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
        player.CManager.DisregardLastHit();
    }

    public void Run()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < knockBackWindow)
        {
            player.VelocityX = knockBackCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
        }
        else
        {
            player.VelocityX = 0;
        }
    }
}
