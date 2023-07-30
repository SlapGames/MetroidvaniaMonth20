using UnityEngine;

public class Telekinesis1ActionState: IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private CombatManager combatManager;
    private bool activated = false;
    public Telekinesis1ActionState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        combatManager = player.CManager;
    }
    
    public void EnterState()
    {
        animator.Play($"Base Layer.Tele 1 Action");
        player.HaltYMovement();
    }

    public void EvaluateTransitions()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            player.ChangeActiveState(nameof(Telekinesis1WinddownState));
        }
    }

    public void ExitState()
    {
        player.UnHaltYMovement();
    }

    public void Run()
    {
        if(!activated && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .5f)
        {
            activated = true;
            player.Telekinesis1Manager.HandleAction();
        }
    }
}
