using UnityEngine;

public class PsychicPowerWindupState: IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private CombatManager combatManager;

    public PsychicPowerWindupState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        combatManager = player.CManager;
    }

    public void EnterState()
    {
        //No windups yet, so we'll just transition out immediately
        EvaluateTransitions();
    }

    public void EvaluateTransitions()
    {
        if (player.CManager.LastHit != null)
        {
            player.ChangeActiveState(nameof(HitStunState));
        }
        else
        {
            player.ChangeActiveState(nameof(PsychicPowerState));
        }
    }

    public void ExitState()
    {
    }

    public void Run()
    {
    }
}
