using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUsed : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private GameObject _hatenaLogos;
    private Material[] _materials;

    void materialChange()
    {
        _hatenaLogos.SetActive(false);
        _materials = _renderer.materials;
        _materials[0] = _material;
        _renderer.materials = _materials;
    }
}
