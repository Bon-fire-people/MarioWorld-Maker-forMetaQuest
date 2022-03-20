using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObject : MonoBehaviour
{
    public BoxCollider connCollider;
    [SerializeField] private BoxCollider[] sideCollider;
    [SerializeField] private GameObject setQuads;
    public RayCast raycast;

    private void Start()
    {
        GameObject obj = GameObject.Find("RayCastControl");
        raycast = obj.GetComponent<RayCast>();
    }

    private void Update()
    {
        if (raycast.isMove == false || raycast.isDelete == false)
        {
            if(setQuads.activeSelf == false) setQuads.SetActive(true);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < sideCollider.Length; i++)
        {
            sideCollider[i].enabled = true;
        }
    }
}
