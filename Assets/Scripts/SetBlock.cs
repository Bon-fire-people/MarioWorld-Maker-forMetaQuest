using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBlock : MonoBehaviour
{
    public GameObject Cube;
    public RayCast rayCast;
    [SerializeField] private Renderer renderer;
    [SerializeField] private Renderer sideRenderer;
    [SerializeField] private Material clearMaterial;
    [SerializeField] private Material hoverMaterial;

    private void Start()
    {
        GameObject obj = GameObject.Find("RayCastControl");
        rayCast = obj.GetComponent<RayCast>();
    }

    private void Update()
    {
        if(rayCast != null)
        {
            if(rayCast.conName == gameObject.transform.parent.parent.gameObject.name + "_" + gameObject.name)
            {
                renderer.material = hoverMaterial;
                sideRenderer.material = hoverMaterial;
            }
            else
            {
                renderer.material = clearMaterial;
                sideRenderer.material = clearMaterial;
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.CompareTag("Box"))
        {
            Cube = collision.transform.parent.gameObject;
            BlockObject b_obj = Cube.GetComponent<BlockObject>();
            if (b_obj.connCollider == null) b_obj.connCollider = gameObject.GetComponent<BoxCollider>();
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
