Shader "Custom/BackgroundToColour"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Shadow Colour", Color) = (1,1,1,1)
		_BackgroundColor("Background Colour", Color) = (1,1,1,1)
		_ClearColor ("Greenscreen Colour", Color) = (0,1,0,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent+2" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		GrabPass {
			"_BackgroundTexture"
		}

		GrabPass {
			"_GreenscreenTexture"
		}

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				fixed4 grabPos  : TEXCOORD1;
			};
			
			fixed4 _Color;
			fixed4 _ClearColor;
			fixed4 _BackgroundColor;
			sampler2D _BackgroundTexture;
			sampler2D _GreenscreenTexture;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.grabPos = ComputeGrabScreenPos(OUT.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 bgcolor = tex2Dproj(_BackgroundTexture, IN.grabPos);
				fixed4 greencolor = tex2Dproj(_GreenscreenTexture, IN.grabPos);
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				if (all(greencolor == _ClearColor)) {
					return bgcolor * (1 - _BackgroundColor.a) + _BackgroundColor;
				}
				return _Color;
			}
		ENDCG
		}
	}
}