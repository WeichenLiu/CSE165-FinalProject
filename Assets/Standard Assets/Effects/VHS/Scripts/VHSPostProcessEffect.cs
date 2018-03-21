using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[RequireComponent (typeof(Camera))]
public class VHSPostProcessEffect : PostEffectsBase {
	Material m;
	public Shader shader;
	public MovieTexture VHS;
    [Range(0, 1)]
    public float mratio = 1.0f;
    [Range(0, 1)]
    public float vratio = 1.0f;

    [Range(0, 1)]
    public float scanlineRange = 0.0f;

    float yScanline, xScanline;

	public void Start() {
		m = new Material(shader);
		m.SetTexture("_VHSTex", VHS);
		VHS.loop = true;
		VHS.Play();
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination){
		yScanline += Time.deltaTime * 0.1f;
		xScanline -= Time.deltaTime * 0.1f;

		if(yScanline >= 1){
			yScanline = Random.value;
		}
		if(xScanline <= 0 || Random.value < 0.05){
			xScanline = Random.value;
		}

	    yScanline *= scanlineRange;
	    xScanline *= scanlineRange;
        m.SetFloat("_yScanline", yScanline);
		m.SetFloat("_xScanline", xScanline);
	    m.SetFloat("_mratio", mratio);
	    m.SetFloat("_vratio", vratio);
        Graphics.Blit(source, destination, m);
	}
}