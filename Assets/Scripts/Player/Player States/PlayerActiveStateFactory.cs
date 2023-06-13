//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PlayerActiveStateFactory
{
    private Player playerToProvide;
    private PlayerInputManager playerInputManagerToProvide;
    private Animator animatorToProvide;

    public PlayerActiveStateFactory(Player playerToProvide, PlayerInputManager playerInputManagerToProvide, Animator animatorToProvide)
    {
        this.playerToProvide = playerToProvide;
        this.playerInputManagerToProvide = playerInputManagerToProvide;
        this.animatorToProvide = animatorToProvide;
    }

    public IPlayerActiveState CreatePlayerActiveState(string name)
    {
        switch (name)
        {
            case nameof(AirDodgeState): 
                return new AirDodgeState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(BackflipState): 
                return new BackflipState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(DodgeState): 
                return new DodgeState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(DoubleJumpState): 
                return new DoubleJumpState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(GrappleDashState): 
                return new GrappleDashState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(GrappleMovementState): 
                return new GrappleMovementState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(GrappleStartupState): 
                return new GrappleStartupState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(HangJumpState): 
                return new HangJumpState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(HangState): 
                return new HangState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(JumpState): 
                return new JumpState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(NoActionState): 
                return new NoActionState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(PivotState): 
                return new PivotState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(RunState): 
                return new RunState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(SkidState): 
                return new SkidState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(SpotDodgeState): 
                return new SpotDodgeState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(JumpHoverState): 
                return new JumpHoverState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(BlockState): 
                return new BlockState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(AttackState): 
                return new AttackState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(AttackWindupState): 
                return new AttackWindupState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(PowerAttackWindupState): 
                return new PowerAttackWindupState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(PowerAttackState): 
                return new PowerAttackState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(ImmobilizedState): 
                return new ImmobilizedState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(PsychicPowerWindupState): 
                return new PsychicPowerWindupState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(PsychicPowerState): 
                return new PsychicPowerState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(BlockStunState): 
                return new BlockStunState(playerToProvide, playerInputManagerToProvide, animatorToProvide);
            case nameof(HitStunState): 
                return new HitStunState(playerToProvide, playerInputManagerToProvide, animatorToProvide);

            default: return null;
        }
    }
}
