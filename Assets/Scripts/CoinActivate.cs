using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinActivate : MonoBehaviour
{
    [SerializeField] private GameObject _obj;

    void coinActivate()
    {
        _obj.SetActive(true);
    }

    void coinDetect()
    {
        _obj.SetActive(false);
    }
}
