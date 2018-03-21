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
    [Range(0, 1)]
    public float ratio = 1.0f;
    [Range(0, 1)]
    public float tratio = 1.0f;

    public Texture2D textTexL;
    public Texture2D textTexR;

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
            gm.SetFloat("_tratio", tratio);
            gm.SetTexture("_TextTexL", textTexL);
            gm.SetTexture("_TextTexR", textTexR);
            Graphics.Blit(source, destination, gm);
        }
        else
        {
            m.SetFloat("_ratio", ratio);
            m.SetFloat("_tratio", tratio);
            m.SetTexture("_TextTexL", textTexL);
            m.SetTexture("_TextTexR", textTexR);
            Graphics.Blit(source, destination, m);
        }
    }
}
