using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    public class BasicEnemy : MonoBehaviour
    {
        public IBasicEnemyActiveState ActiveState { get; private set; }
        public float Speed { get => speed; private set => speed = value; }
        public float PointReachedDistance { get => pointReachedDistance; private set => pointReachedDistance = value; }
        public float VelocityX { get; set; }
        public Transform[] PatrolPoints { get => patrolPoints; private set => patrolPoints = value; }
        public Transform Target { get => target; private set => target = value; }

        public CombatManager CManager { get; private set; }
        public float SpriteDirection { get; private set; }
        public float AttackDelay { get => attackDelay; private set => attackDelay = value; }

        [SerializeField] private float speed;
        [SerializeField] private float pointReachedDistance;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform target;
        [SerializeField] private float closeEnoughDistance;
        [SerializeField] private float noticeDistance;
        [SerializeField] private float unnoticeDistance;
        [SerializeField] private float attackDelay = 1f;
        private BasicEnemyActiveStateFactory activeStateFactory;

        [Space(15)]
        [Header("Debug")]
        public float DEBUG_VelocityX;

        private void Start()
        {
            activeStateFactory = new BasicEnemyActiveStateFactory(this, animator);

            ChangeActiveState(nameof(IdleState));

            rb = GetComponent<Rigidbody2D>();

            CManager = GetComponent<CombatManager>();
        }

        public void ChangeActiveState(string newState)
        {
            ActiveState?.ExitState();

            ActiveState = activeStateFactory.CreateBasicEnemyActiveState(newState);
            if (ActiveState == null)
            {
                throw new System.Exception("The state factory returned null for state: " + newState);
            }

            ActiveState.EnterState();
        }

        private void Update()
        {
            ActiveState.Run();
            ActiveState.EvaluateTransitions();


            DEBUG_VelocityX = VelocityX;
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(VelocityX, rb.velocity.y);
        }

        public void SetDirection(float direction)
        {
            GetComponent<SpriteRenderer>().flipX = direction < 0;
            SpriteDirection = direction;
        }

        public bool IsTargetWithinNoticeDistance()
        {
            return Vector2.Distance(transform.position, target.position) <= noticeDistance;
        }
        public bool IsTargetCloseEnough()
        {
            return Vector2.Distance(transform.position, target.position) <= closeEnoughDistance;
        }
        public bool IsTargetOutsideUnNoticeDistance()
        {
            return Vector2.Distance(transform.position, target.position) >= unnoticeDistance;
        }
    }
}
