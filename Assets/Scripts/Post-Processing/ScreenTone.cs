using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTone : PostEffectsBase {

    public Shader screenToneShader;
    private Material screenToneMaterial = null;

    public Material material {
        get {
            screenToneMaterial = CheckShaderAndCreateMaterial(screenToneShader, screenToneMaterial);
            return screenToneMaterial;
        }
    }

    public Color MulColor = Color.white;
    public Color AddColor = Color.black;


    [Range(1, 8)]
    public int downSample = 2;

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if(material != null) {
            int rtW = src.width / downSample;
            int rtH = src.height / downSample;

            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            buffer0.filterMode = FilterMode.Bilinear;

            // compress
            Graphics.Blit(src, buffer0);

            material.SetColor("_MulColor", MulColor);
            material.SetColor("_AddColor", AddColor);

            Graphics.Blit(buffer0, dest, material, 0);
            RenderTexture.ReleaseTemporary(buffer0);
        }
        else {
            Graphics.Blit(src, dest);
        }
    }
}
