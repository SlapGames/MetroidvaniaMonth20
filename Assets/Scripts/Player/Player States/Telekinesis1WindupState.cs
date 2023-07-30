using UnityEngine;

public class Telekinesis1WindupState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private CombatManager combatManager;

    private bool activated = false;
    private bool wasActiveAlready = false;

    public Telekinesis1WindupState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        combatManager = player.CManager;
    }

    public void EnterState()
    {
        animator.Play($"Base Layer.Tele 1 Windup");
        player.HaltYMovement();
        wasActiveAlready = player.Telekinesis1Manager.WindupWasSuccessful;
    }

    public void EvaluateTransitions()
    {
        if (!player.PowerAvailable)
        {
            player.ChangeActiveState(nameof(NoActionState));
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            if (!wasActiveAlready && player.Telekinesis1Manager.WindupWasSuccessful)
            {
                player.ChangeActiveState(nameof(Telekinesis1ActionState));
            }
            else
            {
                player.ChangeActiveState(nameof(NoActionState));
            }
        }
    }

    public void ExitState()
    {
        player.UnHaltYMovement();
    }

    public void Run()
    {
        if (!activated && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .5)
        {
            activated = true;
            player.Telekinesis1Manager.HandleWindup();
        }

    }
}
