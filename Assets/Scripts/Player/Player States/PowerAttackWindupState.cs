using UnityEngine;

public class PowerAttackWindupState: IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private CombatManager combatManager;

    public PowerAttackWindupState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        combatManager = player.CManager;
    }

    public void EnterState()
    {
        player.HaltPreviousMovement();

        combatManager.AttackKey = "Power";
        Attack currentAttack = combatManager.GetCurrentAttack();
        animator.Play($"Base Layer.{currentAttack.Name} Wind Up");
    }

    public void EvaluateTransitions()
    {
        if (player.CManager.LastHit != null)
        {
            player.ChangeActiveState(nameof(HitStunState));
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            player.ChangeActiveState(nameof(PowerAttackState));
        }
    }

    public void ExitState()
    {
        player.UnHaltYMovement();

        if (playerInputManager.Move != 0)
        {
            player.SetSpriteDirection(Mathf.Sign(playerInputManager.Move));
        }
    }

    public void Run()
    {
    }
}
