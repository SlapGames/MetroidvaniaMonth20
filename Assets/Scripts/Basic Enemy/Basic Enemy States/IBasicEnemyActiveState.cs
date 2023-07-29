using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasicEnemyActiveState
{
    public void EnterState();
    public void Run();
    public void EvaluateTransitions();
    public void ExitState();

}
