using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 透明度渐变
/// </summary>
public class SetImageAlpha : PostEffectsBase
{

    [Range(0, 1)]
    public float leftX = 0;
    [Range(0, 1)]
    public float rightX = 0;
    [Range(0, 1)]
    public float topY = 0;
    [Range(0, 1)]
    public float bottomY = 0;
    [Range(-2, 0)]
    public float alphaSmooth = 0;

    public Shader alphaShader;
    private Material _materal;
    public Material _Material
    {
        get
        {
            _materal = CheckShaderAndCreateMaterial(alphaShader, _materal);
            return _materal;
        }
    }
    private void Awake()
    {
        alphaShader = Shader.Find("Custom/ImageAlpha");
    }
    void Update()
    {
        _Material.SetFloat("_AlphaLX", leftX * 2f);
        _Material.SetFloat("_AlphaRX", ((1 - rightX) - 0.5f) * 2);
        _Material.SetFloat("_AlphaTY", ((1 - topY) - 0.5f) * 2);
        _Material.SetFloat("_AlphaBY", bottomY * 2);
        _Material.SetFloat("_AlphaPower", alphaSmooth);
        //变量的计算只是为了映射范围
        //GetComponent<Sprite>().material = _Material;
        GetComponent<SpriteRenderer>().material = _Material;
        GetComponent<SpriteRenderer>().sharedMaterial.renderQueue = 3000;
    }

}
