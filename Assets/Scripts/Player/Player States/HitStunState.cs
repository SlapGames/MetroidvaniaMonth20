using UnityEngine;

public class HitStunState: IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private CombatManager combatManager;

    private VelocityCalculator knockBackCalc;
    private float knockBackWindow = .4f;

    public HitStunState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        combatManager = player.CManager;
    }

    public void EnterState()
    {
        animator.Play($"Base Layer.Hit Stun");

        player.SetSpriteDirection(-combatManager.LastHit.Direction);

        player.VelocityX = 0;
        knockBackCalc = new VelocityCalculator(combatManager.LastHit.Direction * combatManager.LastHit.KnockBack, -1);

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
        player.CManager.DisregardLastHit();
    }

    public void Run()
    {
        //TODO: Make the knockback more consistent. Maybe use raycasts to deternine how far the player should be knocked back? Or maybe Rigidbody.MoveTowards
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