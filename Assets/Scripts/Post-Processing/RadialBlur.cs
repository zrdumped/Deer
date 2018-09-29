using System.Collections;
using UnityEngine;

public class RadialBlur : PostEffectsBase {

    public Shader radialBlurShader;
    private Material radialBlurMaterial = null;
    public Material material {
        get {
            radialBlurMaterial =
                CheckShaderAndCreateMaterial(radialBlurShader, radialBlurMaterial);
            return radialBlurMaterial;
        }
    }

    [Range(0.001f, 2.0f)]
    public float sampleDist;

    [Range(0.001f, 7.0f)]
    public float sampleStrength;

    [Range(1,12)]
    public int downSample = 4;



    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if(material != null) {
            material.SetFloat("_SampleDist", sampleDist);
            material.SetFloat("_SampleStrength", sampleStrength);

            int rtW = src.width / downSample;
            int rtH = src.height / downSample;
            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            buffer0.filterMode = FilterMode.Bilinear;

            Graphics.Blit(src, buffer0, material, 0);

            material.SetTexture("_BlurTex", buffer0);

            Graphics.Blit(src, dest, material, 1);
            RenderTexture.ReleaseTemporary(buffer0);
        }
    }
}
