using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{

    public Color inactive;
    public Color active;
    public bool isActive = false;
    public float gradientLength;
    public float time = 3.0f;
    private Renderer r;
    private MeshFilter mf;

    // Use this for initialization
    void Start ()
    {
        r = GetComponent<Renderer>();
	    Material m = r.material;
	    r.material = Instantiate(m);
        mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.mesh;
        mf.mesh = Instantiate(mesh);
        var uv = mesh.uv;
        var colors = new Color[uv.Length];
        int i = 0;
        for (; i < uv.Length; i++)
        {
            colors[i] = inactive;
        }
        mf.mesh.colors = colors;
    }

    public void activate(bool flag = true)
    {
        isActive = flag;
        if (isActive)
        {
            StartCoroutine("setGradientColorOverTime", Time.time);
        }
    }
    

    IEnumerator setGradientColorOverTime(float start)
    {
        var mesh = mf.mesh;
        var uv = mesh.uv;
        var colors = new Color[uv.Length];
        while (Time.time - start < time)
        {
            // Instead if vertex.y we use uv.x
            int startIndex = (int) (uv.Length * (Time.time - start) / time);
            int i = 0;
            for (; i < startIndex; i++)
            {

                colors[i] = active;
            }

            for (; i < startIndex + gradientLength && i < uv.Length; i++) { 
                colors[i] = Color.Lerp(active, inactive, uv[i].x);
            }

            for (; i < uv.Length; i++)
            {
                colors[i] = inactive;
            }
            mesh.colors = colors;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
