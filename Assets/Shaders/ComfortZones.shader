Shader "Custom/ComfortZones"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ColorPolish("Color Polish", Color) = (0,0,0,0)
	}
		SubShader
	{
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent"}

		ZWrite Off
		Cull Off

		LOD 100

		Pass
		{
			Stencil {
				Ref 1
				Comp Greater
				Pass replace
				Fail IncrSat
			}
			Blend Zero One
		}

		Pass
		{
			Stencil {
				Ref 1
				Comp Equal
			}			

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float4 color : COLOR; 
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}