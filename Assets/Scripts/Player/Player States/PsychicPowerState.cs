﻿using UnityEngine;

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
    }

    public void EvaluateTransitions()
    {
        if (player.ActivePsychicPower == "")
        {
            player.ChangeActiveState(nameof(NoActionState));
        }
        else if (player.ActivePsychicPower == "Telekinesis 1")
        {
            player.ChangeActiveState(nameof(Telekinesis1WindupState));
        }
    }

    public void ExitState()
    {
    }

    public void Run()
    {
    }
}