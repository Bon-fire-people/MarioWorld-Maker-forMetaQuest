using System;
using OculusSampleFramework;
using UnityEngine;


public class BlockHit : MonoBehaviour
{

    public OVRHand _ovrLeftHand;
    public OVRHand _ovrRightHand;
    [SerializeField] private Animator _BoxAnimator;
    [SerializeField] private Animator _KinokoAnimator;
    [SerializeField] private Animator _FlowerAnimator;
    [SerializeField] private Animator _CoinAnimator;
    private float _handTime = 0;

    private (OVRHand hand, string handName) getCollisionHand(Collision other)
    {
        try
        {
            //親子関係 OVRHandPrefab/Capsules/Hand_Index1_***
            GameObject targetObject = other.transform.parent.parent.gameObject;
            OVRHand rightHand = HandsManager.Instance.RightHand;
            OVRHand leftHand = HandsManager.Instance.LeftHand;
            if (_handTime > 0 ) return (null, "None");
            if (targetObject.Equals(leftHand.gameObject))
            {
                _handTime = Time.deltaTime;
                return (leftHand, "LeftHand");
            }
            if (targetObject.Equals(rightHand.gameObject))
            {
                _handTime = Time.deltaTime;
                return (rightHand, "RightHand");
            }
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

        int _rndCnt = UnityEngine.Random.Range(1, 4);

        _BoxAnimator.SetTrigger("HitTrigger");

        switch (_rndCnt) 
        {
            case 1:
                _KinokoAnimator.SetTrigger("HitTrigger");
                break;

            case 2:
                _FlowerAnimator.SetTrigger("HitTrigger");
                break;

            case 3:
                _CoinAnimator.SetTrigger("HitTrigger");
                break;

        }

        gameObject.SetActive(false);
    }
    
}
