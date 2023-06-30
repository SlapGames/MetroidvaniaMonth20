using UnityEngine;

public class PowerAttackState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    private CombatManager combatManager;
    private Attack instantiatedAttack;
    private VelocityCalculator stepForwardCalc;

    public PowerAttackState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;

        combatManager = player.CManager;
    }

    public void EnterState()
    {
        player.HaltPreviousMovement();

        Attack currentAttack = combatManager.GetCurrentAttack();
        animator.Play($"Base Layer.{currentAttack.Name}");

        instantiatedAttack = GameObject.Instantiate(currentAttack, player.transform.position, Quaternion.identity, player.transform);
        instantiatedAttack.Initialize(player.gameObject);
        if (player.SpriteDirection >= 0)
        {
            instantiatedAttack.transform.localScale = Vector3.one;
        }
        else
        {
            instantiatedAttack.transform.localScale = new Vector3(-1, 1, 1);
        }

        stepForwardCalc = new VelocityCalculator(player.SpriteDirection * instantiatedAttack.StepForwardValue, -1);

        instantiatedAttack.TriggerAttack();
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
        player.UnHaltYMovement();

        if (instantiatedAttack != null)
        {
            GameObject.Destroy(instantiatedAttack.gameObject);
        }
    }

    public void Run()
    {
        //TODO: the movement from attacks doesn't look great, so thatll need to be tuned up at some point
        player.VelocityX = stepForwardCalc.CalculateNextVelocity(player.VelocityX, player.DeltaTimeCopy);
    }
}
