using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    public class ChaseState : IBasicEnemyActiveState
    {
        private enum State
        {
            Noticing,
            Run,
            StandStill
        }

        private BasicEnemy enemy;
        private Animator animator;

        private VelocityCalculator velocityCalculator;
        private State currentState = State.Noticing;

        public ChaseState(BasicEnemy enemyToProvide, Animator animatorToProvide)
        {
            enemy = enemyToProvide;
            animator = animatorToProvide;
        }

        public void EnterState()
        {
            animator.Play("Notice");
            enemy.VelocityX = 0;

            velocityCalculator = new VelocityCalculator(enemy.Speed, -1);
            velocityCalculator.TargetVelocity *= Mathf.Sign(enemy.Target.position.x - enemy.transform.position.x);
            enemy.SetDirection(velocityCalculator.TargetVelocity);
        }

        public void Run()
        {
            if (currentState == State.Noticing && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                animator.Play("Run");
                currentState = State.Run;
            }
            else if (currentState == State.Run)
            {
                velocityCalculator = new VelocityCalculator(enemy.Speed, -1);
                velocityCalculator.TargetVelocity *= Mathf.Sign(enemy.Target.position.x - enemy.transform.position.x);
                enemy.SetDirection(velocityCalculator.TargetVelocity);

                enemy.VelocityX = velocityCalculator.CalculateNextVelocity(enemy.VelocityX, Time.deltaTime);
                if (enemy.IsTargetCloseEnough())
                {
                    animator.Play("Idle");
                    currentState = State.StandStill;
                    enemy.VelocityX = 0;
                }
                //else
                //{
                //}
            }
            //else if (currentState == State.StandStill)
            //{
            //    if (!enemy.IsTargetCloseEnough())
            //    {
            //        animator.Play("Run");
            //        currentState = State.Run;
            //    }

            //    enemy.SetDirection(enemy.Target.position.x - enemy.transform.position.x);
            //}
        }

        public void EvaluateTransitions()
        {
            if (currentState == State.StandStill)
            {
                enemy.ChangeActiveState(nameof(AttackState));
            }
        }

        public void ExitState()
        {
        }
    }
}
