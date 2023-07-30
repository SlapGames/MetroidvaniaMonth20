using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace CustomCamera
{
    public class AtlasCamera : MonoBehaviour
    {
        [Serializable]
        public struct CameraPositionStats
        {
            public Vector2 offset;

            /// <summary>
            /// the x value is the min position for the x axis, and the y value is the max
            /// </summary>
            public Vector2 xExtremes;
            /// <summary>
            /// the x value is the min position for the y axis, and the y value is the max
            /// </summary>
            public Vector2 yExtremes;
        }
        [Serializable]
        public struct CameraBehaviors
        {
            public bool haltOffsetChangingWithDirection;
        }

        public CameraPositionStats CurrentCameraPositionStats { get => currentCameraPositionStats; set => currentCameraPositionStats = value; }
        public CameraBehaviors CurrentCameraBehaviors { get => currentCameraBehaviors; set => currentCameraBehaviors = value; }

        [SerializeField] private Transform toFollow;
        [SerializeField] private float speedMin = 2;
        [SerializeField] private float distanceFromTargetForMaxSpeed = 1;
        [SerializeField] private float distanceFromTargetForCatchUpSpeed = 5;
        [SerializeField] private float speedUpMult = 2;
        [SerializeField] private float catchUpMult = 4;
        [SerializeField] private float speedUpDelay = 1f;

        [Space(15)]
        [Header("Positioning")]
        [SerializeField] private CameraPositionStats currentCameraPositionStats;
        [SerializeField] private CameraPositionStats defaultCameraPositionStats;

        [Space(15)]
        [Header("Behavior")]
        [SerializeField] private CameraBehaviors currentCameraBehaviors;
        [SerializeField] private CameraBehaviors defaultCameraBehaviors;

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

            ReturnToDefaults();
        }

        private void LateUpdate()
        {
            float xOffsetSign = toFollowRenderer.flipX && !CurrentCameraBehaviors.haltOffsetChangingWithDirection ? -1 : 1;
            float yOffsetSign = lookDown ? -1 : 1;
            float toFollowSpeed = (toFollow.position - toFollowLastPosition).magnitude / Time.deltaTime;
            float speed;

            float distanceFromTarget = Vector2.Distance((Vector2)transform.position - new Vector2(CurrentCameraPositionStats.offset.x * xOffsetSign, CurrentCameraPositionStats.offset.y * yOffsetSign), toFollow.position);

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

            targetPosition = new Vector3(toFollow.position.x + xOffsetSign * CurrentCameraPositionStats.offset.x, toFollow.position.y + yOffsetSign * CurrentCameraPositionStats.offset.y, transform.position.z);
            KeepTargetPositionWithinBounds();
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);


            if (Time.time > speedUpTimer)
            {
                speedUp = false;
            }

            toFollowLastPosition = toFollow.position;

            lastLookAheadValue = xOffsetSign;
        }

        private void KeepTargetPositionWithinBounds()
        {
            float xComponent = Mathf.Clamp(targetPosition.x, CurrentCameraPositionStats.xExtremes.x, CurrentCameraPositionStats.xExtremes.y);
            float yComponent = Mathf.Clamp(targetPosition.y, CurrentCameraPositionStats.yExtremes.x, CurrentCameraPositionStats.yExtremes.y);

            targetPosition = new Vector3(xComponent, yComponent, transform.position.z);
        }

        public void ReturnToDefaults()
        {
            CurrentCameraPositionStats = defaultCameraPositionStats;
            CurrentCameraBehaviors = defaultCameraBehaviors;
        }
    }
}
