using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera))]
public class DarkenEffect : PostEffectsBase
{
    Material m;
    public Shader shader;
    Material gm;
    public Shader gshader;

    public float ratio = 1.0f;

    public bool greyscale = false;

    // Use this for initialization
    void Start()
    {
        m = new Material(shader);
        gm = new Material(gshader);
    }

    // Update is called once per frame
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (greyscale)
        {
            gm.SetFloat("_ratio", ratio);
            Graphics.Blit(source, destination, gm);
        }
        else
        {
            m.SetFloat("_ratio", ratio);
            Graphics.Blit(source, destination, m);
        }
    }
}
