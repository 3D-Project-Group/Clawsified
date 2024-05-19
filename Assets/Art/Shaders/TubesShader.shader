Shader "MyShaders/MyShader01"
{
    Properties
    {
        //Waves Control
        _Velocity("Velocity of wave", float) = 3
        _Displacement("Displacement of vertices", float) = 0.5
        _Smoothness("Smoothness of the waves", float) = 2
        
        //Movement Control
        _TopThreshold("Top Vertices Threshold", float) = 0.5
        _ScaleFactor("Scale Factor", float) = 0.1 
        
        _Tint("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            #include "UnityCG.cginc"

            //Waves Control
            float _Velocity;
            float _Displacement;
            float _Smoothness;

            //Movment Control
            float _TopThreshold;
            float _ScaleFactor;

            float4 _Tint;
            
            // To save the data of each vertex
            struct VertexData {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Interpolators of vertex and fragment
            struct Interpolators
            {
                float4 position : POSITION;
                float3 localposition : TEXCOORD0;
            };

            Interpolators MyVertexProgram(VertexData vd)
            {
                Interpolators i;
                i.position = vd.position; // Set the position to the position of the vertex
                i.localposition = vd.position / 5;

                // Only modify vertices above the threshold
                if (vd.position.y >= _TopThreshold)
                {
                    i.position.y += sin((_Time.x * _Velocity) + (vd.position.x * _Smoothness)) * _Displacement * _ScaleFactor;
                }

                i.position = UnityObjectToClipPos(i.position);
                return i;
            }

            float4 MyFragmentProgram(Interpolators i) : SV_Target
            {
                return float4(i.localposition + _Tint, 0.5);
            }
            
            ENDCG
        }
    }
}
