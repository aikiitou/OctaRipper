Shader "Unlit/Glitch01"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlitchSpeed ("GlitchSpeed" , Float) = 10
        _Rate ("NoiseRate" , Float) = 0.98
        _GlitchValue ("GlitchValue" ,Float) = 1
        _GlitchAlpha ("GlitchAlpha" , Float) = 0.3
        _BlockNum ("BlockNum" , Float) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On
        LOD 100

        CGINCLUDE

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _GlitchSpeed;
            float _Rate;
            float _GlitchValue;
            float _GlitchAlpha;
            float _BlockNum;

            float random(float2 p)
            {
                return frac(sin(dot(p,float2(12.9898,78.233))) * 43758.5453);
            }

            float blockNoise(float2 st,float t)
            {
                fixed2 p = floor(st.y * _BlockNum + t);

                return random(p);
            }

        ENDCG


        Pass
        {
            Tags{ "LightMode" = "UniversalForward" }
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                if(abs(sin(floor(_Time.y))) >= _Rate)
                {
                    float t = _Time.y * _GlitchSpeed;
                    float n = blockNoise(i.uv,t * _BlockNum);

                    if(n >= 0.6) discard;

                    col.r = 0;
                    col.g = col.g;
                    col.b = 0;

                    col.a = _GlitchAlpha;
                }
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
        Pass
        {
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                if(abs(sin(floor(_Time.y))) >= _Rate)
                {
                    float4 offset = float4(_GlitchValue,0,0,0);
                    o.vertex += offset;
                }
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                if(abs(sin(floor(_Time.y))) >= _Rate)
                {
                    float t = _Time.y * _GlitchSpeed;
                    float n = blockNoise(i.uv,t * _BlockNum);

                    if(n >= 0.6) discard;

                    col.r = col.r;
                    col.g = 0;
                    col.b = 0;

                    col.a = _GlitchAlpha;
                }
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
