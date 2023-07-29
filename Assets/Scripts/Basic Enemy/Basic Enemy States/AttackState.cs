using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy 
{
    public class AttackState : IBasicEnemyActiveState
    {
        private BasicEnemy enemy;
        private Animator animator;

        private CombatManager combatManager;
        private float attackTimer = 0;
        private bool firstAttack = true;

        public AttackState(BasicEnemy enemyToProvide, Animator animatorToProvide)
        {
            this.enemy = enemyToProvide;
            this.animator = animatorToProvide;

            combatManager = enemy.CManager;
        }

        public void EnterState()
        {
            enemy.VelocityX = 0;
        }

        public void Run()
        {
            if (firstAttack || animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                animator.Play("Neutral");

                //We don't clean up immediately after the attack to make debugging a bit easier in the inspector (pause to look at the newly instantiated attack, stuff like that.)
                combatManager.Cleanup();


                attackTimer += Time.deltaTime;
                if (firstAttack || attackTimer >= enemy.AttackDelay)
                {
                    firstAttack = false;
                    attackTimer = 0;
                    Attack();
                }
            }
            else if(combatManager.StepForwardCalc != null)
            {
                enemy.VelocityX = combatManager.StepForwardCalc.CalculateNextVelocity(enemy.VelocityX, Time.deltaTime);
            }
        }

        public void EvaluateTransitions()
        {
            if(enemy.IsTargetCloseEnough() == false && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                enemy.ChangeActiveState(nameof(ChaseState));
            }
        }

        public void ExitState()
        {
            combatManager.Cleanup();
        }

        private void Attack()
        {
            animator.Play("Attack");

            combatManager.SetupNextAttack(enemy.SpriteDirection);
            combatManager.TriggerAttack();
        }
    }
}

