using UnityEngine;

public class Telekinesis1WinddownState: IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private CombatManager combatManager;

    public Telekinesis1WinddownState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        combatManager = player.CManager;
    }
    
    public void EnterState()
    {
        animator.Play($"Base Layer.Tele 1 Winddown");
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
    }

    public void Run()
    {
    }
}
