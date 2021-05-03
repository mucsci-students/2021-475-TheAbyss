    Shader "Gloria/PortalCard" {
        Properties
        {
            _Color ("Color", Color) = (0.5,0.5,0.5,0.5)
            _MainTex ("Texture (RGBA)", 2D) = "white" {}
            _Depth ("Depth Fade Distance", Range(0.01,200.0)) = 1.0
            _FadeLength("Fade Length", Float) = 10.0
            _FallOffU("Falloff U", float) = 0.0
            _FallOffV("Falloff V", float) = 0.0
        }
     
        SubShader
        {
            Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
            LOD 200
            Blend SrcAlpha OneMinusSrcAlpha
            Lighting Off ZWrite Off
     
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog;
                #pragma target 3.0
     
                #include "UnityCG.cginc"
     
                fixed4 _Color;
                float _Depth;
                float _FadeLength;
                sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _CameraDepthTexture;
                float4 _CameraDepthTexture_ST;
                float _FallOffU;
                float _FallOffV;
     
                struct appdata{
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                };
     
                struct v2f{
                    float2 uv : TEXCOORD0;
                    float3 dist :TEXCOORD3;
                    float4 projPos : TEXCOORD2;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };
     
                v2f vert (appdata i){
                    v2f o;
                    o.vertex = UnityObjectToClipPos(i.vertex);
                    o.uv = i.texcoord;
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    o.dist = UnityObjectToViewPos(i.vertex);
     
                    o.projPos = ComputeScreenPos(o.vertex);
                    UNITY_TRANSFER_DEPTH(o.projPos);
     
                    return o;
                }
     
                fixed4 frag(v2f i) : SV_TARGET
                {
                    const float PI = 3.14159;
                    fixed4 col = _Color * tex2D(_MainTex, i.uv);
                    float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                    float depthFactor = (sceneZ - length(i.dist)) / _Depth;
                    float distFactor = length(i.dist)/_FadeLength;

                    float falloffU = pow((sin(i.uv.x * PI) + 1) / 2, _FallOffU);
                    float falloffV = pow((sin(i.uv.y * PI) + 1) / 2, _FallOffV);

                    col.a *= saturate(distFactor * depthFactor);
                    col.a *= falloffU * falloffV;
                                   
                    UNITY_APPLY_FOG(i.fogCoord, col);
                    return col;
                   
                }
     
                ENDCG
            }
     
        }
        FallBack "Diffuse"
    }
     
