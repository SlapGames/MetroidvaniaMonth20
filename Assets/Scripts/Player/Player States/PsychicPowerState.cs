using UnityEngine;

public class PsychicPowerState: IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private CombatManager combatManager;

    public PsychicPowerState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        combatManager = player.CManager;
    }

    public void EnterState()
    {
        animator.Play($"Base Layer.Power");
    }

    public void EvaluateTransitions()
    {
        if (player.CManager.LastHit != null)
        {
            player.ChangeActiveState(nameof(HitStunState));
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            if (playerInputManager.ReadCurrentInput()?.InputType == InputType.Attack)
            {
                player.ChangeActiveState(nameof(AttackWindupState));
            }
            else
            {
                player.ChangeActiveState(nameof(NoActionState));
            }
        }
    }

    public void ExitState()
    {
    }

    public void Run()
    {
    }
}
