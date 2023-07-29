using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    public class AttackWindupState : IBasicEnemyActiveState
    {
        private BasicEnemy enemyToProvide;
        private Animator animatorToProvide;

        public AttackWindupState(BasicEnemy enemyToProvide, Animator animatorToProvide)
        {
            this.enemyToProvide = enemyToProvide;
            this.animatorToProvide = animatorToProvide;
        }

        public void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public void Run()
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
    }
}
