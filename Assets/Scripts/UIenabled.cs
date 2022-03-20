using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIenabled : MonoBehaviour
{
    private Transform _iniTransform;
    private Animator _animator;
    private float _time;

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _iniTransform.position = new Vector3(0f, 0.12f, 0.14f);
        _iniTransform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {

        if (OVRInput.IsControllerConnected(OVRInput.Controller.Touch) == true)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.X))
            {
                _animator.SetTrigger("open");
                gameObject.transform.position = new Vector3(0f, 0.12f, 0.14f);
                gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            }

            if (OVRInput.Get(OVRInput.RawButton.X))
            {
                gameObject.transform.position = new Vector3(0f, 0.12f, 0.14f);
                gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            }


            if (OVRInput.GetUp(OVRInput.RawButton.X))
            {
                _animator.SetTrigger("close");
                gameObject.transform.position = Vector3.zero;
                gameObject.transform.localScale = Vector3.zero;
            }
        }
        
    }
}
