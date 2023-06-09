﻿using UnityEngine;

public class ImmobilizedState: IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;
    private Animator animator;

    public ImmobilizedState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
        this.animator = animator;
    }

    public void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public void EvaluateTransitions()
    {
        throw new System.NotImplementedException();
    }

    public void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public void Run()
    {
        throw new System.NotImplementedException();
    }
}
