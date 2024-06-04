Shader "MyShaders/Myshader02" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowPower ("Glow Power", Range(0.1, 10)) = 1
    }

    SubShader {
        Tags { "RenderType"="Opaque" }

        Pass {
            CGPROGRAM
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            #include "UnityCG.cginc"

            struct VertexData {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _GlowPower;
            float4 _GlowColor;

            Interpolators MyVertexProgram (VertexData vd) {
                Interpolators i;
                i.vertex = UnityObjectToClipPos(vd.vertex);
                i.uv = TRANSFORM_TEX(vd.uv, _MainTex);
                return i;
            }

            fixed4 MyFragmentProgram (Interpolators i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = col.rgb * _GlowColor.rgb * _GlowPower;
                col.a = col.a * _GlowColor.a;
                return col;
            }
            ENDCG
        }
    }
}