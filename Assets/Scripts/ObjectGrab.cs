using System;
using OculusSampleFramework;
using UnityEngine;

public class ObjectGrab : MonoBehaviour
{
    [SerializeField] private Material _default;
    [SerializeField] private Material isGrab;
    [SerializeField] private Material isCollision;
    [SerializeField] private OVRHand _ovrLeftHand;
    [SerializeField] private OVRHand _ovrRightHand;
    private Rigidbody _rigidBody;
    private Renderer _render;
    private Vector3 _initPosition;
    private Vector3 _targetPosition;
    private Quaternion _initRotation;

    private float _time;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _render = GetComponent<Renderer>();
        _initPosition = this.transform.position;
        _initRotation = this.transform.rotation;
    }

    private void resetVelocity()
    {
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
    }


    private (bool isPinching, Vector3 position) isPinchingHand(OVRHand hand)
    {
        Vector3 position = Vector3.zero;
        bool isPinching = false;

        if (hand.GetFingerIsPinching(OVRHand.HandFinger.Index)
            || hand.GetFingerIsPinching(OVRHand.HandFinger.Middle)
            || hand.GetFingerIsPinching(OVRHand.HandFinger.Ring))
        {
            position = hand.PointerPose.position;
            isPinching = true;
        }

        return (isPinching, position);
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


    // <summary>
    /// 触れた時
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        var collisionHand = getCollisionHand(other);
        if (collisionHand.hand == null) return;

        _render.material = isCollision;

        var result = isPinchingHand(collisionHand.hand);
        if (!result.isPinching) return;
        _rigidBody.freezeRotation = true;
        _targetPosition = result.position;
        _targetPosition.y = _targetPosition.y - 0.03f;
        this.transform.position = _targetPosition;
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
            _targetPosition.y = _targetPosition.y - 0.03f;
            this.transform.position = _targetPosition;
            _render.material = isGrab;
        }

    }

    /// <summary>
    /// 離れた時
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionExit(Collision other)
    {
        var collisionHand = getCollisionHand(other);
        if (collisionHand.hand == null) return;

        _rigidBody.freezeRotation = false;
        _render.material = _default;
        resetVelocity();
    }

    private void Update()
    {
        if (_time <= 3)
        {
            if (_ovrLeftHand.GetFingerIsPinching(OVRHand.HandFinger.Index)
                || _ovrRightHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
            {
                _time += Time.deltaTime;
            }
        }
        else
        {
            this.transform.position = _initPosition;
            this.transform.rotation = _initRotation;
            _time = 0;
        }
    }

}
