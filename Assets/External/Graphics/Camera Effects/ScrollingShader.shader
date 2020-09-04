Shader "Custom/ScrollingShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex("Mask Texutre", 2D) = "white" {}
        _ScrollXSpeed("X", Range(-100,100)) = 0
        _ScrollYSpeed("Y", Range(-100,100)) = 0
        [Enum(UnityEngine.Rendering.BlendOp)] _BlendOp("BlendOp", Float) = 0
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("BlendSource", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("BlendDestination", Float) = 0
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 0
        [Toggle] _ZWrite("ZWrite", Float) = 0
    }

    SubShader
    {
        BlendOp [_BlendOp]
        Blend [_SrcBlend] [_DstBlend]
        ZWrite [_ZWrite]
        Cull [_Cull]

        Tags
        {
            "Queue" = "Transparent+1"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

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

            sampler2D _MainTex;
            sampler2D _MaskTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            half _ScrollXSpeed;
            half _ScrollYSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 scrolledUV = TRANSFORM_TEX(i.uv, _MainTex);
                fixed xScrollValue = _ScrollXSpeed * _Time;
                fixed yScrollValue = _ScrollYSpeed * _Time;

                scrolledUV += fixed2(xScrollValue, yScrollValue);
                fixed4 col = tex2D(_MainTex, scrolledUV);
                col *= _Color;
                col.a *= tex2D(_MaskTex, i.uv).r;
                return col;
            }
            ENDCG
        }
    }
}
