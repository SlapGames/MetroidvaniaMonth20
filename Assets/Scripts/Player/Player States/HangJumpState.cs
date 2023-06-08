using UnityEngine;

public class HangJumpState : IPlayerActiveState
{
    private Player player;
    private PlayerInputManager playerInputManager;

    public HangJumpState(Player player, PlayerInputManager playerInputManager, Animator animator)
    {
        this.player = player;
        this.playerInputManager = playerInputManager;
    }

    public void EnterState()
    {

    }

    public void EvaluateTransitions()
    {
    }

    public void ExitState()
    {
    }

    public static string GetName()
    {
        return "HangJump";
    }

    public void Run()
    {
    }
}

