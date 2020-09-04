Shader "Custom/ExplosionShader"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (1,1,1,1)
        _MainTex ("Noise Texture 1", 2D) = "gray" {}
        _Texture1("Noise Texture 2", 2D) = "gray" {}
        _ScrollXSpeed("X", Range(-100,100)) = 0
        _ScrollYSpeed("Y", Range(-100,100)) = 0
        _BrightnessMaskValue("Brightness Mask Value", Range(0, 10)) = 1
        _VertexOffset("VertexOffset", Range(-1, 1)) = 0.05
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
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _Texture1;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _AuxColor;
            float _BrightnessMaskValue;
            half _ScrollXSpeed;
            half _ScrollYSpeed;
            float _VertexOffset;

            v2f vert (appdata v)
            {
                v2f o;
                float4 tempUv = TRANSFORM_TEX(v.uv, _MainTex).xyxy;
                tempUv.zw = float2(0, 1);

                fixed4 scrolledUV1 = tempUv;

                fixed xScrollValue = _ScrollXSpeed * _Time;
                fixed yScrollValue = _ScrollYSpeed * _Time;

                scrolledUV1.xy += fixed2(xScrollValue, yScrollValue);
                fixed4 col1 = tex2Dlod(_MainTex, scrolledUV1);

                fixed4 scrolledUV2 = tempUv;
                scrolledUV2.xy += fixed2(xScrollValue, -yScrollValue);
                fixed4 col2 = tex2Dlod(_Texture1, scrolledUV2);

                fixed4 col = col1 * col2;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex += _VertexOffset * v.normal * col.r;

                o.uv = tempUv;
                o.uv2 = v.uv2;
                o.color = col;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                fixed brightnessAdjust = pow(i.uv.y / _MainTex_ST.y, _BrightnessMaskValue);
                fixed4 col = floor((i.color + brightnessAdjust) * 5) * 0.2;
                col *= _Color;
                col.a *= i.uv2.x;
                return col;
            }
            ENDCG
        }
    }
}
