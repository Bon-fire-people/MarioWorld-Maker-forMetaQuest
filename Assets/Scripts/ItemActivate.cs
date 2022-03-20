using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActivate : MonoBehaviour
{
    [SerializeField] private GameObject _obj;
    private BoxCollider _bc;
    private Rigidbody _rb;


    void Start()
    {
        _bc = _obj.GetComponent<BoxCollider>();
        _rb = _obj.GetComponent<Rigidbody>();
    }

    void itemActivate()
    {
        _obj.SetActive(true);
    }

    void rbActivate()
    {
       _bc.enabled = true;
       _rb.isKinematic = false;
    }
}
