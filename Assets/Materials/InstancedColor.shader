Shader "Custom/InstancedColor"
{
    SubShader 
    {
        CGINCLUDE
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
        ENDCG

        Pass
        {
            Tags { "RenderType" = "Opaque" "LightMode"="ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile_fwdbase

            struct v2f {
                float4 pos      : SV_POSITION;
                fixed4 color    : COLOR;
                float2 uv       : TEXCOORD0;
                SHADOW_COORDS(1)
            }; 

            float map(float v, float min1, float max1, float min2, float max2) {
                return min2 + (v - min1) * (max2 - min2) / (max1 - min1);
            }

            float4 _Colors[1023];   // Max instanced batch size.

            v2f vert(appdata_full v, uint instanceID: SV_InstanceID) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v)
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = _Colors[instanceID];

                TRANSFER_SHADOW(o)

                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 col = i.color;
                fixed4 shadow = map(SHADOW_ATTENUATION(i), 0, 1, 0.5, 1);
                col *= shadow;
                return col;
            }
            ENDCG
        }

        Pass
        {
            Tags { "LightMode"="ShadowCaster" }

            CGPROGRAM
            #pragma vertex vert_shadow
            #pragma fragment frag_shadow
            #pragma multi_compile_instancing
            #pragma multi_compile_shadowcaster

            struct v2f {
                V2F_SHADOW_CASTER;
            };

            v2f vert_shadow(appdata_base v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v)
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag_shadow(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}