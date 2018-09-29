using System.Collections;
using UnityEngine;

public class EyesOpen : PostEffectsBase {

    public Shader eyesOpenShader;
    public Texture mask;

    [Range(0.0f, 0.20f)]
    public float Delta;

    // Blur iterations - larger number means more blur.
    [Range(0, 4)]
    public int iterations = 3;

    // Blur spread for each iteration - larger value means more blur
    [Range(0.2f, 3.0f)]
    public float blurSpread = 0.6f;

    [Range(1, 8)]
    public int downSample = 2;




    private Material eyesOpenMaterial = null;

    public Material material {
        get {
            eyesOpenMaterial = CheckShaderAndCreateMaterial(eyesOpenShader, eyesOpenMaterial);
            return eyesOpenMaterial;
        }
    }

    // Mask controlling threshod - control the extent to which the eye open
    [Range(0, 1.0f)]
    public float threshod;

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if(material != null && mask != null) {


            int rtW = src.width / downSample;
            int rtH = src.height / downSample;

            // 新建 buffer 储存大小为 rtW、rtH 的图片
            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            // 设置 downsample 的过滤模式为双线性
            buffer0.filterMode = FilterMode.Bilinear;

            // 这里使用了 downsampling 的技术，把图片先压缩再模糊处理，优化性能
            // 把源图片缩放并传给 buffer0（不经过 shader 处理）
            Graphics.Blit(src, buffer0);

            for(int i = 0; i < iterations; i++) {
                material.SetFloat("_BlurSize", 1.0f + i * blurSpread);

                RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);

                // Render the vertical pass
                // 调用 shader 中的第 0 个 Pass
                Graphics.Blit(buffer0, buffer1, material, 0);

                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
                buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);

                // Render the horizontal pass
                // 调用 shader 中的第 1 个 Pass
                Graphics.Blit(buffer0, buffer1, material, 1);

                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
            }

            //Graphics.Blit(buffer0, dest);
            



            material.SetTexture("_MaskTex", mask);
            material.SetFloat("_Threshod", threshod);
            material.SetFloat("_Delta", Delta);
            Graphics.Blit(buffer0, dest, material);

            RenderTexture.ReleaseTemporary(buffer0);
        }
        else {
            Graphics.Blit(src, dest);
        }
    }
}
