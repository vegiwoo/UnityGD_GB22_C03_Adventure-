using System;
using System.Collections;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace C03_Adventure
{
    [RequireComponent(typeof(Animator))]
    public class IKControl : MonoBehaviour
    {
        #region Links
        [SerializeField] private bool ikActive;
        
        [Header("Hands")]
        [SerializeField] private Transform leftHandTarget;
        [SerializeField] private Transform rightHandTarget;
        [SerializeField, Tooltip("in meters")] private float handContactDistance = 0.45f;
        [Header("Head")]
        [SerializeField] private Transform headTurnTarget;
        [SerializeField, Tooltip("in meters")] private float headAimDistance = 2.0f;
        #endregion

        #region Constants and variables 
        private Animator _animator;
        
        private const int Weight = 1;
        private const int NullWeight = 1;
        
        float state = 0;
        float elapsedTime = 0;
        public float timeReaction = 0.5f;
        #endregion
        
        #region MonoBehaviour methods

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        // Called when calculating IK
        private void OnAnimatorIK(int layerIndex)
        {
            if (_animator && ikActive)
            {
                // Setting look target for head
                if (headTurnTarget != null)
                {
                    var isTurnHead = Vector3.Distance(transform.position, headTurnTarget.position) < headAimDistance;
                    if (isTurnHead)
                    {
                        _animator.SetLookAtWeight(Weight);
                        _animator.SetLookAtPosition(headTurnTarget.position);
                    }
                }

                var position = transform.position;
                var leftHandTp = leftHandTarget.position;
                var rightHandTp = rightHandTarget.position;

                // Setting target for left hand and put it in position
                var isTurnLeftHand = Vector3.Distance(position, leftHandTp) < handContactDistance;
                if (leftHandTarget != null && isTurnLeftHand)
                {
                    SetIKWeightPositionRotation(AvatarIKGoal.LeftHand, Weight, leftHandTp, leftHandTarget.rotation);
                }

                // Setting target for right hand and put it in position
                var isTurnRightHand = Vector3.Distance(position, rightHandTp) < handContactDistance;
                if (rightHandTarget != null && isTurnRightHand)
                {
                    SetIKWeightPositionRotation(AvatarIKGoal.RightHand, Weight, rightHandTp, rightHandTarget.rotation);
                }
            }
            else
            {
                _animator.SetLookAtWeight(NullWeight);
                SetIKWeightPositionRotation(AvatarIKGoal.LeftHand, NullWeight);
                SetIKWeightPositionRotation(AvatarIKGoal.RightHand, NullWeight);
            }
        }

        #endregion
        
        #region Functionality

        private void SetIKWeightPositionRotation(AvatarIKGoal goal, float weight, Vector3? position = null, Quaternion? rotation = null)
        {
            _animator.SetIKPositionWeight(goal, weight);
            _animator.SetIKRotationWeight(goal, weight);

            if (position == null || rotation == null) return;
            
            _animator.SetIKPosition(goal, position.Value);
            _animator.SetIKRotation(goal, rotation.Value);
        }
        #endregion
    }
}