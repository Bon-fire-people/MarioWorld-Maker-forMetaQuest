using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetQuadsObject : MonoBehaviour
{
    public RayCast raycast;

    private void Start()
    {
        GameObject obj = GameObject.Find("RayCastControl");
        raycast = obj.GetComponent<RayCast>();
    }

    private void Update()
    {
        if(raycast.isMove || raycast.isDelete)
        {
            gameObject.SetActive(false);
        }
        else gameObject.SetActive(true);
    }
}
