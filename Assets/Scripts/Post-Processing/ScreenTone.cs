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

    public Color color;

    void OnRenderImage(RenderTexture src, RenderTexture dest) {

    }
}
