public interface IPlayerActiveState 
{
    public void EnterState();
    public void Run();
    public void EvaluateTransitions();
    public void ExitState();
}

