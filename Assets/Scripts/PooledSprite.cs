using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledSprite : MonoBehaviour
{
    public GameObject Face;
    public GameObject Rear;

    private MeshRenderer _faceRenderer;
    private MeshRenderer _rearRenderer;

    public void SetMaterial(Material material)
    {
        _faceRenderer.material = material;
        _rearRenderer.material = material;
    }

    public void Centre()
    {
        this.gameObject.transform.position = new Vector3(0, 0, 0);
    }

    void Awake()
    {
        _faceRenderer = Face.GetComponent<MeshRenderer>();
        _rearRenderer = Rear.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
