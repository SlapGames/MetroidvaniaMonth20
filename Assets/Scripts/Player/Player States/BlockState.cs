using UnityEngine;

public class BlockState: IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private CombatManager combatManager;

    public BlockState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        combatManager = player.CManager;
    }

    public void EnterState()
    {
        player.HaltPreviousMovement();
        animator.Play($"Base Layer.Block");
    }

    public void EvaluateTransitions()
    {
        if (combatManager.LastHit != null)
        {
            player.ChangeActiveState(nameof(BlockStunState));
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            player.ChangeActiveState(nameof(NoActionState));
        }
    }

    public void ExitState()
    {
        player.UnHaltYMovement();
    }

    public void Run()
    {
    }
}
