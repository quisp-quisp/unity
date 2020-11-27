// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Skybox"
{
    Properties
    {
        _Tint("Tint Color", Color) = (.5, .5, .5, 1)
        [Gamma] _Exposure("Exposure", Range(0, 8)) = 1.0
        _Rotation("Rotation", Range(0, 360)) = 0
        _BlendCubemaps("Blend Cubemaps", Range(0, 1)) = 0.5
        [NoScaleOffset] _Tex("Cubemap (HDR)", Cube) = "grey" {}
        [NoScaleOffset] _Tex2("Cubemap (HDR) 2", Cube) = "grey" {}
    }
        SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            half4 _Tint;
            half4 _Tex_HDR;
            half _Exposure;
            float _Rotation;
            float _BlendCubemaps;
            samplerCUBE _Tex;
            samplerCUBE _Tex2;

            float3 RotateAroundYInDegrees(float3 vertex, float degrees)
            {
                float alpha = degrees * UNITY_PI / 180.0;
                float sina, cosa;
                sincos(alpha, sina, cosa);
                float2x2 m = float2x2(cosa, -sina, sina, cosa);
                return float3(mul(m, vertex.xz), vertex.y).xzy;
            }

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                float3 rotated = RotateAroundYInDegrees(v.vertex, _Rotation);
                o.vertex = UnityObjectToClipPos(rotated);
                o.texcoord = v.vertex.xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 env1 = texCUBE(_Tex, i.texcoord);
                float4 env2 = texCUBE(_Tex2, i.texcoord);
                float4 env = lerp(env1, env2, _BlendCubemaps);
                half3 c = DecodeHDR(env, _Tex_HDR);
                c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
                c *= _Exposure;
                return half4(c, 1);
            }
            ENDCG
        }
    }
    FallBack Off
}
