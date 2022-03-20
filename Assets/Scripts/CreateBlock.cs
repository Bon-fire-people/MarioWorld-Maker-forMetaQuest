using System;
using OculusSampleFramework;
using UnityEngine;

public class CreateBlock : MonoBehaviour
{
    [SerializeField] private OVRHand _ovrLeftHand;
    [SerializeField] private OVRHand _ovrRightHand;
    [SerializeField] private OVRSkeleton _ovrLeftSkelton;
    [SerializeField] private OVRSkeleton _ovrRightSkelton;
    [SerializeField] private GameObject _hatenaBlock;
    [SerializeField] private GameObject _wallBlock;
    private float _leftTime;
    private Transform _leftTransform;
    private Vector3 _targetPosition;

    public bool _isGrab = false;
    public bool _isAttach = false;

    private float _rightTime;
    private Transform _rightTransform;

    private void Update()
    {
        #region MakeBlock
        if (_leftTime <= 3)
        {
            if (_ovrLeftHand.GetFingerIsPinching(OVRHand.HandFinger.Ring))
            {
                _leftTime += Time.deltaTime;
                _leftTransform = _ovrLeftSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_RingTip].Transform;
            }
        }else
        {
            _targetPosition = _leftTransform.position;
            _targetPosition.y = _targetPosition.y - 0.05f;
            var obj = Instantiate(_hatenaBlock, _targetPosition, Quaternion.identity);
            _leftTime = 0;
        }

        if (_rightTime <= 3)
        {
            if (_ovrRightHand.GetFingerIsPinching(OVRHand.HandFinger.Ring))
            {
                _rightTime += Time.deltaTime;
                _rightTransform = _ovrRightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_RingTip].Transform;
            }
        }else
        {
            _targetPosition = _rightTransform.position;
            _targetPosition.y = _targetPosition.y - 0.05f;
            var obj = Instantiate(_wallBlock, _targetPosition, Quaternion.identity);
            _rightTime = 0;
        }
        #endregion

     }
}
