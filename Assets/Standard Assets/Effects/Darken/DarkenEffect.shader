Shader "Hidden/DarkenEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
      uniform sampler2D _TextTexL;
      uniform sampler2D _TextTexR;
      float _ratio;
      float _grey;
      float _tratio;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				//col.rgb = 1 - col.rgb;
        fixed4 text;
        if (unity_CameraProjection[0][2] < 0)
        {
          text = tex2D(_TextTexL, i.uv);
        }
        else
        {
          text = tex2D(_TextTexR, i.uv);
        }
				return col * _ratio + text * _tratio;
			}
			ENDCG
		}
	}
}
