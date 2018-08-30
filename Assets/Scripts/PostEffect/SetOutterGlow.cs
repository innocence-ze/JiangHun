using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 外发光
/// </summary>
public class SetOutterGlow : PostEffectsBase
{
    [Range(0, 10)]
    public float factor = 1;

    [Range(0, 10)]
    public float samplerRange = 7;

    public Shader outterGlowShader;
    private Material _material;
    public Material _Material
    {
        get
        {
            _material = CheckShaderAndCreateMaterial(outterGlowShader, _material);
            return _material;
        }
    }
    private void Awake()
    {
        outterGlowShader = Shader.Find("Custom/OutterGlow");

    }
    private void Update()
    {
        _Material.SetFloat("_Factor", factor);
        _Material.SetFloat("_SamplerRange", samplerRange);
        GetComponent<SpriteRenderer>().material = _Material;
        GetComponent<SpriteRenderer>().sharedMaterial.color = new Color(0, 1, 0, 1);
        GetComponent<SpriteRenderer>().sharedMaterial.renderQueue = 3000;
    }

}