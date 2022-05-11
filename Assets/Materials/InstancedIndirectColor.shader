Shader "Custom/InstancedIndirectColor"
{
    SubShader 
    {
        CGINCLUDE
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            #include "UnityLightingCommon.cginc"
            struct MeshProperties {
                float4x4 mat;
                float4 color;
            };
            StructuredBuffer<MeshProperties> _Properties;
        ENDCG

        Pass 
        {
            Tags { "RenderType" = "Opaque" "LightMode"="ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase_fullshadows

            struct v2f {
                float4 pos          : SV_POSITION;
                fixed4 color        : COLOR;
                SHADOW_COORDS(0)
            };

            v2f vert(appdata_full i, uint instanceID : SV_InstanceID) {
                MeshProperties props = _Properties[instanceID];

                v2f o;
                float4 pp = mul(props.mat, i.vertex);
                o.pos = UnityObjectToClipPos(pp);
                o.color = props.color;

                TRANSFER_SHADOW(o)

                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                return i.color;
            }
            ENDCG
        }

        Pass
        {
            Tags { "LightMode"="ShadowCaster" }
            
            CGPROGRAM
            #pragma vertex vert_shadow
            #pragma fragment frag_shadow
            #pragma multi_compile_shadowcaster

            struct v2f_shadow { 
                V2F_SHADOW_CASTER;
            };

            v2f_shadow vert_shadow(appdata_full v, uint instanceID : SV_InstanceID)
            {
                MeshProperties props = _Properties[instanceID];

                v2f_shadow o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)

                float4 pp = mul(props.mat, v.vertex);
                o.pos = UnityObjectToClipPos(pp);
                return o;
            }

            float4 frag_shadow(v2f_shadow i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}