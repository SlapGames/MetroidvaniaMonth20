using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    public class IdleState : IBasicEnemyActiveState
    {
        private BasicEnemy enemy;
        private Animator animator;

        private VelocityCalculator velocityCalculator;
        private int targetPatrolPoint = -1;

        public IdleState(BasicEnemy enemyToProvide, Animator animatorToProvide)
        {
            this.enemy = enemyToProvide;
            this.animator = animatorToProvide;
        }

        public void EnterState()
        {
            animator.Play("Idle");

            if(enemy.PatrolPoints.Length > 1)
            {
                targetPatrolPoint = 0;

                velocityCalculator = new VelocityCalculator(enemy.Speed, -1);
                velocityCalculator.TargetVelocity *= Mathf.Sign(enemy.PatrolPoints[targetPatrolPoint].position.x - enemy.transform.position.x);
                enemy.SetDirection(velocityCalculator.TargetVelocity);
            }
        }

        public void Run()
        {
            if(targetPatrolPoint != -1)
            {
                animator.Play("Run");

                float nextVelocity = velocityCalculator.CalculateNextVelocity(enemy.VelocityX, Time.deltaTime);
                enemy.VelocityX = nextVelocity;

                if(Vector2.Distance(enemy.transform.position, enemy.PatrolPoints[targetPatrolPoint].position) <= enemy.PointReachedDistance)
                {
                    targetPatrolPoint++;
                    if(targetPatrolPoint >= enemy.PatrolPoints.Length)
                    {
                        targetPatrolPoint = 0;
                    }

                    velocityCalculator.Acceleration = -1;
                    velocityCalculator.TargetVelocity = enemy.Speed;
                    velocityCalculator.TargetVelocity *= Mathf.Sign(enemy.PatrolPoints[targetPatrolPoint].position.x - enemy.transform.position.x);
                    enemy.SetDirection(velocityCalculator.TargetVelocity);
                }
            }
        }

        public void EvaluateTransitions()
        {
            if (enemy.IsTargetWithinNoticeDistance())
            {
                enemy.ChangeActiveState(nameof(ChaseState));
            }
        }

        public void ExitState()
        {
        }
    }
}