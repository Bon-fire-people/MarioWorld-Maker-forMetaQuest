using System;
using OculusSampleFramework;
using UnityEngine;

public class BlockSort : MonoBehaviour
{
    private GameObject _gameObject;
    private Rigidbody _rigidbody;
    private CreateBlock createBlock;
    private Vector3 _targetPosition;
    
    private void Start()
    {
        _gameObject = gameObject.transform.parent.gameObject;
    }


    private (OVRHand hand, string handName) getCollisionHand(Collision other)
    {
        try
        {
            //親子関係 OVRHandPrefab/Capsules/Hand_Index1_***
            GameObject targetObject = other.transform.parent.parent.gameObject;
            OVRHand rightHand = HandsManager.Instance.RightHand;
            OVRHand leftHand = HandsManager.Instance.LeftHand;
            if (targetObject.Equals(leftHand.gameObject)) return (leftHand, "LeftHand");
            if (targetObject.Equals(rightHand.gameObject)) return (rightHand, "RightHand");
            return (null, "None");
        }
        catch (Exception e)
        {
            //parentが無かった時のエラーをキャッチ
            return (null, "None");
        }
    }

    private (bool isPinching, Vector3 position) isPinchingHand(OVRHand hand)
    {
        Vector3 position = Vector3.zero;
        bool isPinching = false;

        if (hand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            position = hand.PointerPose.position;
            isPinching = true;
        }

        return (isPinching, position);
    }


    // <summary>
    /// 触れた時
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        var collisionHand = getCollisionHand(other);
        if (collisionHand.hand == null) return;
        var result = isPinchingHand(collisionHand.hand);
        if (!result.isPinching) return;
        _targetPosition = result.position;
        _targetPosition.y = _targetPosition.y - 0.05f;
        _gameObject.transform.position = _targetPosition;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    /// <summary>
    /// 触れている間
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionStay(Collision other)
    {
        var collisionHand = getCollisionHand(other);
        if (collisionHand.hand == null) return;

        var result = isPinchingHand(collisionHand.hand);
        if (result.isPinching)
        {
            _targetPosition = result.position;
            _targetPosition.y = _targetPosition.y - 0.05f;
            _gameObject.transform.position = _targetPosition;
        }
    }

    /// <summary>
    /// 離れた時
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionExit(Collision other)
    {
        var collisionHand = getCollisionHand(other);
        createBlock._isGrab = false;
        if (collisionHand.hand == null) return;
        _rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
