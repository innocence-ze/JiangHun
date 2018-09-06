using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SetMultiply : PostEffectsBase
{
    public RenderTexture renderTexture;
    public RenderTexture renderTexture2;
    public Shader curShader;
    public Texture2D blendTexture;
    public float blendOpacity = 1.0f;
    private Material curMaterial;
    public Material CurMaterial
    {
        get
        {
            curMaterial = CheckShaderAndCreateMaterial(curShader, curMaterial);
            return curMaterial;
        }
    }
    private void Awake()
    {
        curShader = Shader.Find("Custom/ColorCompute");
    }

    //void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    //{
    //    if (curShader != null)
    //    {
    //        curMaterial.SetTexture("_BlendTex", blendTexture);
    //        curMaterial.SetFloat("_Opacity", blendOpacity);

    //        Graphics.Blit(sourceTexture, destTexture, curMaterial);
    //    }
    //    else
    //    {
    //        Graphics.Blit(sourceTexture, destTexture);
    //    }
    //}

    private void Update()
    {
        GetComponent<SpriteRenderer>().material = CurMaterial;

        int width = renderTexture.width;
        int height = renderTexture.height;
        Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture2D.Apply();

        int width2 = renderTexture2.width;
        int height2 = renderTexture2.height;
        Texture2D texture2D2 = new Texture2D(width, height, TextureFormat.ARGB32, false);
        RenderTexture.active = renderTexture2;
        texture2D.ReadPixels(new Rect(0, 0, width2, height2), 0, 0);
        texture2D.Apply();

        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_MainTex", texture2D);
        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_Texture2", texture2D2);

        //blendOpacity = Mathf.Clamp(blendOpacity, 0.0f, 1.0f);
        GetComponent<SpriteRenderer>().sharedMaterial.renderQueue = 3000;
    }


}
