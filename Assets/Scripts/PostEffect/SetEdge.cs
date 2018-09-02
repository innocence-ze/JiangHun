using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEdge : PostEffectsBase
{
    [Range(0, 1)]
    public float offsetUV = 0f;

    [Range(0, 1)]
    public float treshold = 0.5f;

    public Shader edgeShader;
    private Material _material;
    public Material _Material
    {
        get
        {
            _material = CheckShaderAndCreateMaterial(edgeShader, _material);
            return _material;
        }
    }

    private void Awake()
    {
        edgeShader = Shader.Find("Custom/Edge");

    }

    private void Update()
    {
        _Material.SetFloat("_OffsetUV", offsetUV);
        _Material.SetFloat("_AlphaTreshold", treshold);
        GetComponent<SpriteRenderer>().material = _Material;

        GetComponent<SpriteRenderer>().sharedMaterial.renderQueue = 3000;
    }

}
