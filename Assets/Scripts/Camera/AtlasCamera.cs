using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace CustomCamera
{
    public class AtlasCamera : MonoBehaviour
    {
        [SerializeField] private Transform toFollow;
        [SerializeField] private Vector2 offset;
        [SerializeField] private float speedMin = 2;
        [SerializeField] private float distanceFromTargetForMaxSpeed = 1;
        [SerializeField] private float distanceFromTargetForCatchUpSpeed = 5;
        [SerializeField] private float speedUpMult = 2;
        [SerializeField] private float catchUpMult = 4;
        [SerializeField] private float speedUpDelay = 1f;

        private SpriteRenderer toFollowRenderer;
        private Vector3 targetPosition;
        private Vector3 toFollowLastPosition;
        private bool lookDown;
        private float lastLookAheadValue;
        private bool speedUp;
        private bool catchUp;
        private float speedUpTimer = 0;

        private void Start()
        {
            toFollowRenderer = toFollow.GetComponent<SpriteRenderer>();
            toFollowLastPosition = toFollow.position;

            lastLookAheadValue = toFollowRenderer.flipX ? -1 : 1;
        }

        private void LateUpdate()
        {
            float xOffsetSign = toFollowRenderer.flipX ? -1 : 1;
            float yOffsetSign = lookDown ? -1 : 1;
            float toFollowSpeed = (toFollow.position - toFollowLastPosition).magnitude / Time.deltaTime;
            float speed;

            float distanceFromTarget = Vector2.Distance((Vector2)transform.position - new Vector2(offset.x * xOffsetSign, offset.y * yOffsetSign), toFollow.position);

            catchUp = distanceFromTarget >= distanceFromTargetForCatchUpSpeed;
            if (catchUp)
            {
                speedUp = false;
                speedUpTimer = 0;
            }
            else if(lastLookAheadValue != xOffsetSign)
            {
                speedUp = true;
                speedUpTimer = Time.time + speedUpDelay;
            }


            //We remove the offset from the camera position so that we can make adjustments to fields liek distanceFromTargetForMaxSpeed independent
            //of the offset.
            if (distanceFromTarget < distanceFromTargetForMaxSpeed)
            {
                speed = speedMin;
            }
            else
            {
                speed = Mathf.Max(toFollowSpeed, speedMin);
            }

            if (catchUp)
            {
                speed *= catchUpMult;
            }
            else if (speedUp)
            {
                speed *= speedUpMult;
            }

            targetPosition = new Vector3(toFollow.position.x + xOffsetSign * offset.x, toFollow.position.y + yOffsetSign * offset.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Time.time > speedUpTimer)
            {
                speedUp = false;
            }

            toFollowLastPosition = toFollow.position;

            lastLookAheadValue = xOffsetSign;
        }
    }
}
