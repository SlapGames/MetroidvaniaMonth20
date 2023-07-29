using UnityEngine;

namespace BasicEnemy
{
    public class BasicEnemyActiveStateFactory 
    {
        private BasicEnemy enemyToProvide;
        private Animator animatorToProvide;

        public BasicEnemyActiveStateFactory(BasicEnemy enemyToProvide, Animator animatorToProvide)
        {
            this.enemyToProvide = enemyToProvide;
            this.animatorToProvide = animatorToProvide;
        }

        public IBasicEnemyActiveState CreateBasicEnemyActiveState(string name)
        {
            switch (name)
            {
                case nameof(IdleState): 
                    return new IdleState(enemyToProvide, animatorToProvide);
                case nameof(AttackState): 
                    return new AttackState(enemyToProvide, animatorToProvide);
                case nameof(AttackWindupState): 
                    return new AttackWindupState(enemyToProvide, animatorToProvide);
                case nameof(ChaseState): 
                    return new ChaseState(enemyToProvide, animatorToProvide);
                case nameof(DeadState): 
                    return new DeadState(enemyToProvide, animatorToProvide);
                case nameof(HitState): 
                    return new HitState(enemyToProvide, animatorToProvide);
                default:
                    return null;
            }
        }
    }
}
